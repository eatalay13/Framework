using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class NavigationMenuForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentMenuId { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsExternal { get; set; }
        public string ExternalUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool Visible { get; set; }
        public bool Permitted { get; set; }
    }
}
