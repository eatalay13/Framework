using Core.Infrastructure.PagedList;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseModel, new()
    {
        #region Methods

        TEntity GetById(Guid id);

        IList<TEntity> GetByIds(IList<Guid> ids);

        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);

        IPagedList<TEntity> GetAllPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        void Insert(TEntity entity);

        void Insert(IList<TEntity> entities);

        void Update(TEntity entity);

        void Update(IList<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(IList<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        TEntity LoadOriginalCopy(TEntity entity);

        IList<TEntity> EntityFromSql(string procedureName, params object[] parameters);

        void Truncate(bool resetIdentity = false);

        #endregion

        #region Properties

        IQueryable<TEntity> Table { get; }

        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}