using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Caching;
using Core.Infrastructure.PagedList;
using Entities.Models;
using Microsoft.Data.SqlClient;

namespace Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseModel, new()
    {
        #region Methods

        Task<TEntity> GetByIdAsync(int id);

        Task<IList<TEntity>> GetByIdsAsync(IList<int> ids);

        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);

        Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        Task InsertAsync(TEntity entity);

        Task InsertAsync(IList<TEntity> entities);

        void Update(TEntity entity);

        void Update(IList<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(IList<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> LoadOriginalCopyAsync(TEntity entity);

        Task<IList<TEntity>> FromSqlAsync(string procedureName, params SqlParameter[] parameters);

        Task TruncateAsync(bool resetIdentity = false);

        #endregion

        #region Properties

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}