using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PepperMap.DomainLayer.Interfaces;
using PepperMap.DomainLayer.Models;

namespace PepperMap.Webservice.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {

        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // GET api/routes/{id}
        [HttpGet("{id}")]
        public async Task<Route> Get(string id)
        {
            return await _routeService.GetRouteByNumberAsync(id);
        }

    }
}
