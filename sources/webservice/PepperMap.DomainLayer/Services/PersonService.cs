using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PepperMap.DomainLayer.Interfaces;
using PepperMap.DomainLayer.Mappers;
using PepperMap.Infrastructure.Database;
using PepperMap.Infrastructure.Database.Models;

namespace PepperMap.DomainLayer.Services
{
    public class PersonService : IPersonService
    {

        private readonly DatabaseContext _context;

        public PersonService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Person>> GetPatientAsync()
        {
            return await SelectAll((person) => person.Flag == PersonType.Patient);
        }

        public async Task<IEnumerable<Models.Person>> GetPatientAsync(string filter)
        {
            return await PatientAsync(filter, (person) => person.Flag == PersonType.Patient);
        }

        public async Task<IEnumerable<Models.Person>> GetStaffAsync()
        {
            return await SelectAll((person) => person.Flag == PersonType.Staff);
        }

        public async Task<IEnumerable<Models.Person>> GetStaffAsync(string filter)
        {
            return await PatientAsync(filter, (person) => person.Flag == PersonType.Staff);
        }

        private IIncludableQueryable<Person, Route> GetPersonContext()
        {
            return _context
                .People
                .Include(p => p.Location)
                .Include(p => p.Location.Route);
        }

        private string CleanString(string param)
        {
            return param.Replace(" ", "");
        }

        private async Task<IEnumerable<Models.Person>> PatientAsync(string filter, Func<Person, bool> func)
        {
            filter = CleanString(filter);
            return (await GetPersonContext()
                    .Where(person => func(person)
                                     && (person.Lastname.ToLowerInvariant().Contains(filter)
                                         || person.Firstname.ToLowerInvariant().Contains(filter)
                                         || string.Concat(person.Firstname, person.Lastname).ToLowerInvariant().Contains(filter)
                                         || string.Concat(person.Lastname, person.Firstname).ToLowerInvariant().Contains(filter)))
                    .ToListAsync())
                .Select(PersonHelper.MapPerson);
        }

        private async Task<IEnumerable<Models.Person>> SelectAll(Func<Person, bool> func)
        {
            return (await GetPersonContext()
                    .Where(person => func(person))
                    .ToListAsync())
                .Select(PersonHelper.MapPerson);
        }

    }
}