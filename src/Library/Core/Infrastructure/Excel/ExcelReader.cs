using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Excel
{
    public class ExcelReader : IExcelReader
    {
        public async Task<List<T>> ReadFileAsync<T>(IFormFile formFile)
        {
            await using var stream = new MemoryStream();

            await formFile.CopyToAsync(stream);

            return Read<T>(stream);
        }

        protected List<T> Read<T>(Stream stream)
        {
            List<T> list = new List<T>();

            using var package = new ExcelPackage(stream);

            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            var columnCount = worksheet.Dimension.Columns;

            for (int row = 2; row <= rowCount; row++)
            {
                var model = Activator.CreateInstance<T>();

                var properties = model.GetType().GetProperties();

                for (int col = 1; col <= columnCount; col++)
                {
                    var colName = worksheet.Cells[1, col].Value.ToString()?.Trim();

                    var property = properties.FirstOrDefault(prop => prop.Name.ToLower() == colName.ToLower());

                    if (property is null) continue;
                    object safeValue;

                    var value = worksheet.Cells[row, col].Value;

                    Type valueType = Nullable.GetUnderlyingType(property.PropertyType) ??
                                     property.PropertyType;

                    if (valueType.IsEnum)
                        safeValue = Enum.Parse(valueType, value.ToString() ?? string.Empty);

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

                list.Add(model);
            }

            return list;
        }
    }
}
