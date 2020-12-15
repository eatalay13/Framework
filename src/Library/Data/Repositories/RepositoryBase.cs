using Entities.Models;
using Core.Extensions;
using Core.Infrastructure.PagedList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseModel, new()
    {
        #region Fields

        private readonly DbSet<TEntity> _entities;
        private readonly DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Properties

        private int UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User is null) return 0;

                var userId = _httpContextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId?.ToInt() ?? 0;

            }
        }
        #endregion

        #region Ctor

        public RepositoryBase(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _entities = context.Set<TEntity>();
        }

        #endregion

        #region Methods

        public virtual TEntity GetById(int id)
        {
            return id <= 0 ? null : _entities.Find(id);
        }

        public virtual IList<TEntity> GetByIds(IList<int> ids)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            var query = Table;
            if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) != null)
                query = _entities.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();

            var entries = query.Where(entry => ids.Contains(entry.Id)).ToList();

            var sortedEntries = new List<TEntity>();
            foreach (var id in ids)
            {
                var sortedEntry = entries.FirstOrDefault(entry => entry.Id == id);
                if (sortedEntry != null)
                    sortedEntries.Add(sortedEntry);
            }

            return sortedEntries;
        }

        public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var query = func != null ? func(Table) : Table;
            return query.ToList();
        }

        public virtual IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 1, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = func != null ? func(Table) : Table;

            return new PagedList<TEntity>(query, pageIndex, pageSize, getOnlyTotalCount);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedBy = UserId;
            entity.ModifiedBy = UserId;

            _entities.Add(entity);
        }

        public virtual void Insert(IList<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                entity.CreatedBy = UserId;
                entity.ModifiedBy = UserId;
            }

            using var transaction = new TransactionScope();
            _entities.AddRange(entities);
            transaction.Complete();
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
                Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    _entities.Update(entity);
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

        public virtual TEntity LoadOriginalCopy(TEntity entity)
        {
            return _entities.FirstOrDefault(e => e.Id == entity.Id);
        }

        public virtual IList<TEntity> EntityFromSql(string procedureName, params object[] parameters)
        {
            return _entities.FromSqlRaw(procedureName, parameters).ToList();
        }

        public virtual void Truncate(bool resetIdentity = false)
        {
            _entities.RemoveRange();
        }

        #endregion

        #region Properties

        public virtual IQueryable<TEntity> Table => _entities;

        public IQueryable<TEntity> TableNoTracking => Table.AsNoTracking();

        #endregion
    }
}