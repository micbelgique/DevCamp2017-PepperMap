using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PepperMap.DomainLayer.Interfaces;
using PepperMap.DomainLayer.Models;

namespace PepperMap.Webservice.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {

        private readonly IRouteService _routeService;
        private readonly ILocationService _locationService;

        public LocationsController(IRouteService routeService, ILocationService locationService)
        {
            _routeService = routeService;
            _locationService = locationService;
        }

        // GET api/locations/{id}
        [HttpGet("{id}")]
        public async Task<Route> Get(int id)
        {
            return await _locationService.GetRouteAsync(id);
        }

        // GET api/values/5
        [HttpGet("search/{param}")]
        public async Task<IEnumerable<Route>> Search(string param)
        {
            return await _locationService.GetPublicRoutesAsync(param);
        }


        // GET api/values/5
        [HttpGet("search/{param}/medical")]
        public async Task<IEnumerable<Route>> SearchMedicalRoute(string param)
        {
            return await _locationService.GetMedicalRoutesAsync(param);
        }

    }
}
