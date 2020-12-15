using Entities.Dtos;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Entities.EqualityComparer
{
    public class AuthMenuDtoComparer : IEqualityComparer<AuthNavigationMenuDto>
    {
        public bool Equals(AuthNavigationMenuDto x, AuthNavigationMenuDto y)
        {
            if (x is null || y is null) return false;

            return x.Area == y.Area && x.ControllerName == y.ControllerName && x.ActionName == y.ActionName;
        }

        public int GetHashCode([DisallowNull] AuthNavigationMenuDto obj)
        {
            //Get hash code for the Name field if it is not null.
            int hashMenuName = obj.ControllerName == null ? 0 : obj.ControllerName.GetHashCode();

            int hashArea = obj.Area == null ? 0 : obj.Area.GetHashCode();

            //Get hash code for the Code field.
            int hashMenuCode = obj.ActionName == null ? 0 : obj.ActionName.GetHashCode();

            //Calculate the hash code for the product.
            return hashMenuName ^ hashMenuCode ^ hashArea;
        }
    }
}
