using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        #region Lazy
        //private readonly Lazy<IRepository<NavigationMenu>> _navigationMenuRepo;
        #endregion

        #region RepoInitiate

        //public IRepository<NavigationMenu> NavigationMenuRepo => _navigationMenuRepo.Value;


        #endregion

        #region Ctor
        public UnitOfWork(DbContext context)
        {
            _context = context;

            if (context == null)
            {
                throw new ArgumentNullException("Db Context Can Not Be Null");
            }

            //_navigationMenuRepo = CreateRepo<NavigationMenu>();
        }

        #endregion

        #region Methods

        public void SaveChanges()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private Lazy<IRepository<TModel>> CreateRepo<TModel>() where TModel : BaseModel, new()
        {
            return new Lazy<IRepository<TModel>>(() => new RepositoryBase<TModel>(_context));
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}