using Entities.Models.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.NavigateMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IEnumerable<NavigationMenu> Get()
        {
            var data = _navigateMenuService.GetMenuList();
            return data;
        }
    }
}
