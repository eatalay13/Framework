using Entities.Models.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.NavigateMenu;

namespace WepApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly INavigateMenuService _navigateMenuService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, INavigateMenuService navigateMenuService)
        {
            _logger = logger;
            _navigateMenuService = navigateMenuService;
        }

        [HttpGet]
        public async Task<IEnumerable<NavigationMenu>> Get()
        {
            var data = await _navigateMenuService.GetMenuListAsync();
            return data;
        }
    }
}
