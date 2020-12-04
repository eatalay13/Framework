using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    [Table("AspNetNavigationMenu")]
    public class NavigationMenu : BaseModel
    {
        public string Name { get; set; }

        [ForeignKey("ParentNavigationMenu")]
        public Guid? ParentMenuId { get; set; }

        public string Area { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsExternal { get; set; }

        public string ExternalUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool Visible { get; set; }


        public virtual NavigationMenu ParentNavigationMenu { get; set; }


        [NotMapped]
        public bool Permitted { get; set; }
    }
}