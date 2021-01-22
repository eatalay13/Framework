using Entities.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class RoleDevApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> RoleMenuIds { get; set; }
    }
}
