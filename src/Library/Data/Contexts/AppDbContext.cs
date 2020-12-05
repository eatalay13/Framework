using Entities.Models.Auth;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
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
