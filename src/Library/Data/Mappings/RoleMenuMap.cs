using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models.Menu;

namespace Data.Mappings
{
    public class RoleMenuMap : IEntityTypeConfiguration<RoleMenu>
    {
        public void Configure(EntityTypeBuilder<RoleMenu> builder)
        {
            builder.ToTable("RoleMenus");
            builder.HasKey(c => new { c.RoleId, c.NavigationMenuId });
        }
    }
}
