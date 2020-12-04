using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Auth;
using Entities.Models.Menu;

namespace Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<NavigationMenu> NavigationMenu { get; set; }
        public virtual DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMenuPermission>()
                .HasKey(c => new { c.RoleId, c.NavigationMenuId });


            base.OnModelCreating(builder);
        }
    }
}
