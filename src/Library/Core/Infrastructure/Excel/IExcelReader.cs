using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.Excel
{
    public interface IExcelReader
    {
        Task<List<T>> ReadFileAsync<T>(IFormFile formFile);
    }
}
