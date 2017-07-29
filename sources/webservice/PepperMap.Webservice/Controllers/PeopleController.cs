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
        private readonly IPersonService _personService;

        public PeopleController(IRouteService routeService, IPersonService personService)
        {
            _routeService = routeService;
            _personService = personService;
        }

        [HttpGet("{id}")]
        public async Task<Route> Get(int id)
        {
            return await _routeService.GetPersonAsync(id);
        }

        [HttpGet("search/{param}")]
        public async Task<IEnumerable<Route>> Search(string param)
        {
            return await _routeService.GetPersonAsync(param);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<Person>> Get()
        {
            return await _personService.GetPatientAsync();
        }

        [HttpGet("list/{filter}")]
        public async Task<IEnumerable<Person>> Get(string filter)
        {
            return await _personService.GetPatientAsync(filter);
        }

        [HttpGet("staff/list")]
        public async Task<IEnumerable<Person>> GetStaff()
        {
            return await _personService.GetStaffAsync();
        }

        [HttpGet("staff/list/{filter}")]
        public async Task<IEnumerable<Person>> GetStaff(string filter)
        {
            return await _personService.GetStaffAsync(filter);
        }

    }
}
