using Data.Repositories;
using Entities.Models.Menu;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        #region Lazy

        public IRepository<NavigationMenu> NavigationMenuRepo { get; }

        public IRepository<RoleMenu> RoleMenuRepo { get; }

        #endregion


        #region Ctor
        public UnitOfWork(DbContext context,
            IRepository<NavigationMenu> navigationMenuRepo,
            IRepository<RoleMenu> roleMenuRepo)
        {
            _context = context;

            NavigationMenuRepo = navigationMenuRepo;
            RoleMenuRepo = roleMenuRepo;
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

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}