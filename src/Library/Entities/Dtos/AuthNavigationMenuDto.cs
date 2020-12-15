using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AuthNavigationMenuDto
    {
        public string Name { get; set; }
        public string ParentMenuName { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Visible { get; set; }
        public bool IsTopMenu { get; set; }
    }
}
