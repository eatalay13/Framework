using Core.Extensions;
using Core.Infrastructure.PagedList;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseModel, new()
    {
        #region Fields

        private readonly DbSet<TEntity> _entities;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Properties

        private int UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User is null) return 0;

                var userId = _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId?.ToInt() ?? 0;

            }
        }
        #endregion

        #region Ctor

        public RepositoryBase(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _entities = context.Set<TEntity>();
        }

        #endregion

        #region Methods

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }


        public virtual async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            async Task<IList<TEntity>> getByIdsAsync()
            {
                var query = Table;
                if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) != null)
                    query = Table.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();

                //get entries
                var entries = await query.Where(entry => ids.Contains(entry.Id)).ToListAsync();

                //sort by passed identifiers
                var sortedEntries = new List<TEntity>();
                foreach (var id in ids)
                {
                    var sortedEntry = entries.FirstOrDefault(entry => entry.Id == id);
                    if (sortedEntry != null)
                        sortedEntries.Add(sortedEntry);
                }

                return sortedEntries;
            }

            return await getByIdsAsync();
        }


        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = func != null ? func(Table) : Table;
            return await query.ToListAsync();
        }


        public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = func != null ? func(Table) : Table;

            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }


        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedBy = UserId;
            entity.ModifiedBy = UserId;

            await _entities.AddAsync(entity);
        }

        public virtual async Task InsertAsync(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                entity.CreatedBy = UserId;
                entity.ModifiedBy = UserId;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _entities.AddRangeAsync(entities);
            transaction.Complete();
        }

        public virtual async Task<TEntity> LoadOriginalCopyAsync(TEntity entity)
        {
            return await Table
                .FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(entity.Id));
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.ModifiedBy = UserId;
            entity.ModifiedDate = DateTime.Now;

            _entities.Update(entity);
        }


        public virtual void Update(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (!entities.Any())
                return;

            foreach (var entity in entities)
            {
                entity.ModifiedBy = UserId;
                entity.ModifiedDate = DateTime.Now;
            }

            _entities.UpdateRange(entities);
        }


        public virtual void Delete(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    _entities.UpdateRange(entity);
                    break;

                default:
                    _entities.Remove(entity);
                    break;
            }
        }


        public virtual void Delete(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<ISoftDeletedEntity>().Any())
            {
                foreach (var entity in entities)
                    if (entity is ISoftDeletedEntity softDeletedEntity)
                    {
                        softDeletedEntity.Deleted = true;
                        _entities.Update(entity);
                    }
            }
            else
            {
                _entities.RemoveRange(entities);
            }
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var deleteEntities = _entities.Where(predicate);

            _entities.RemoveRange(deleteEntities);

            return deleteEntities.Count();
        }


        public virtual async Task<IList<TEntity>> FromSqlAsync(string procedureName, params SqlParameter[] parameters)
        {
            return await _entities.FromSqlRaw(procedureName, parameters).ToListAsync();
        }


        public virtual async Task TruncateAsync(bool resetIdentity = false)
        {
            //await Table.(resetIdentity);
        }

        #endregion

        #region Properties

        public virtual IQueryable<TEntity> Table => _entities;

        public virtual IQueryable<TEntity> TableNoTracking => Table.AsNoTracking();

        #endregion
    }
}