using FluentValidation;
using System;

namespace Core.Validation
{
    public static class ValidationTool
    {
        public static void Validate(Type validatorType, object entity)
        {
            var validator = (IValidator)Activator.CreateInstance(validatorType);

            if (validator == null) return;

            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
