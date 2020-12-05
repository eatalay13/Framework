using Core.Helpers;
using Data.Contexts;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Controllers
{
    [Area(AreaDefaults.AdminAreaName)]
    [Authorize(policy: PolicyDefaults.AuthorizationPolicy)]
    public class LogsApiController : Controller
    {
        private AppDbContext _context;

        public LogsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var log = _context.Log.Select(i => new
            {
                i.Message,
                i.Level,
                i.Timestamp,
                i.Exception,
                i.LogEvent,
                i.Id
            });

            return Json(await DataSourceLoader.LoadAsync(log, loadOptions));
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _context.Log.FirstOrDefaultAsync(item => item.Id == key);

            _context.Log.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}