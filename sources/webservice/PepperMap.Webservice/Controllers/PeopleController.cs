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

        private readonly ILocationService _locationService;
        private readonly IPersonService _personService;

        public PeopleController(ILocationService locationService, IPersonService personService)
        {
            _locationService = locationService;
            _personService = personService;
        }

        [HttpGet("{id}")]
        public async Task<Route> Get(int id)
        {
            return await _locationService.GetPersonAsync(id);
        }

        [HttpGet("search/{param}")]
        public async Task<IEnumerable<Route>> Search(string param)
        {
            return await _locationService.GetPersonAsync(param);
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
