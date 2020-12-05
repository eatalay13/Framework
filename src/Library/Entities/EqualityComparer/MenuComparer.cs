using Entities.Models.Menu;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EqualityComparer
{
    public class MenuComparer : IEqualityComparer<NavigationMenu>
    {
        public bool Equals(NavigationMenu x, NavigationMenu y)
        {
            if (x is null || y is null) return false;

            if (x.IsExternal || y.IsExternal)
            {
                return x.ExternalUrl.Equals(y.ExternalUrl);
            }

            return x.Area == y.Area && x.ControllerName == y.ControllerName && x.ActionName == y.ActionName;
        }

        public int GetHashCode([DisallowNull] NavigationMenu obj)
        {
            //Check whether the object is null
            if (obj is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashMenuName = obj.Name == null ? 0 : obj.Name.GetHashCode();

            //Get hash code for the Code field.
            int hashMenuCode = obj.Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashMenuName ^ hashMenuCode;
        }
    }
}
