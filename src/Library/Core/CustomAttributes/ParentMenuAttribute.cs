using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ParentMenuAttribute : Attribute
    {
        public string ParentMenuName { get; }
        public int Order { get; }
        public bool IsVisible { get; }

        public ParentMenuAttribute(string parentMenuName,int order = 1,bool isVisible = true)
        {
            ParentMenuName = parentMenuName;
            Order = order;
            IsVisible = isVisible;
        }
    }
}
