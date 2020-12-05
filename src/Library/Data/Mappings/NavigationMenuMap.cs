using Entities.Models.Menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mappings
{
    public class NavigationMenuMap : IEntityTypeConfiguration<NavigationMenu>
    {
        public void Configure(EntityTypeBuilder<NavigationMenu> builder)
        {
            builder.ToTable("NavigationMenus");

            builder.HasIndex(e => e.ParentMenuId, "IX_NavigationMenu_ParentMenuId");

            builder.HasOne(d => d.ParentMenu)
                .WithMany(p => p.InverseParentMenu)
                .HasForeignKey(d => d.ParentMenuId);
        }
    }
}
