using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Extensions
{
    public static class DevExpModelPopulateExtensions
    {
        public static T PopulateModel<T>(this T model, string values)
        {
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);

            var modelType = typeof(T);

            var properties = modelType.GetProperties();

            foreach (var property in properties)
            {
                if (valuesDict.Contains(property.Name))
                {
                    object safeValue;

                    var value = valuesDict[property.Name];

                    Type valueType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    if (valueType.IsEnum)
                        safeValue = Enum.Parse(valueType, value.ToString());

                    else if (valueType == typeof(Guid))
                    {
                        if (Guid.TryParse(value.ToString(), out var guidValue))
                            safeValue = guidValue;
                        else
                            safeValue = null;
                    }

                    else
                        safeValue = (value == null) ? null : Convert.ChangeType(value, valueType);

                    property.SetValue(model, safeValue);
                }
            }

            return model;
        }

        public static string GetFullErrorMessage(this ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return string.Join(" ", messages);
        }
    }
}
