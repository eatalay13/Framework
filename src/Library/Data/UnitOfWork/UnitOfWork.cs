using Data.Repositories;
using Entities.Models.Menu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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

        public async Task<int> SaveChangesAsync()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var count = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return count;
            }
            catch
            {
                await transaction.RollbackAsync();
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