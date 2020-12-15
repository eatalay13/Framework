using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Core.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MenuItemAttribute : Attribute
    {
        public string Name { get; }
        public int Order { get; }
        public bool IsVisible { get; }

        public MenuItemAttribute(string name, int order = 1, bool isVisible = true)
        {
            Name = name;
            Order = order;
            IsVisible = isVisible;
        }
    }
}
