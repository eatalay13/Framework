using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Entities.Models.Menu;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<NavigationMenu> NavigationMenuRepo { get; }
        void SaveChanges();
    }
}
