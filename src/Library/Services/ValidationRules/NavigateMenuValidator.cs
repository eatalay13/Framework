using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;

namespace Services.ValidationRules
{
    public class NavigateMenuValidator : AbstractValidator<Entities.Models.Menu.NavigationMenu>
    {
        public NavigateMenuValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Name).NotEmpty().Length(3, 20);
        }
    }
}
