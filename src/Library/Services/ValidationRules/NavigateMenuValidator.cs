using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models.Menu;
using FluentValidation;

namespace Services.ValidationRules
{
    public class NavigateMenuValidator : AbstractValidator<NavigationMenu>
    {
        public NavigateMenuValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Name).NotEmpty().Length(3, 20);
        }
    }
}
