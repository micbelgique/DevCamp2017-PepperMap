using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PepperMap.DomainLayer.Interfaces;
using PepperMap.DomainLayer.Models;

namespace PepperMap.Webservice.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {

        private readonly IRouteService _routeService;

        public PeopleController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // GET api/locations/{id}
        [HttpGet("{id}")]
        public async Task<Route> Get(int id)
        {
            return await _routeService.GetPersonAsync(id);
        }

        // GET api/values/5
        [HttpGet("search/{param}")]
        public async Task<IEnumerable<Route>> Search(string param)
        {
            return await _routeService.GetPersonAsync(param);
        }

    }
}
