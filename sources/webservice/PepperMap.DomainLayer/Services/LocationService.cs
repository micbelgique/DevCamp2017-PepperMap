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
using Route = PepperMap.DomainLayer.Models.Route;

namespace PepperMap.DomainLayer.Services
{
    public class LocationService : ILocationService
    {
        private readonly DatabaseContext _context;

        public LocationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> GetPublicRoutesAsync(string location)
        {
            return await RoutesAsync(location, (l) => l.Route.Flag == RouteFlag.Public || l.Route.Flag == RouteFlag.All);
        }

        public async Task<IEnumerable<Route>> GetMedicalRoutesAsync(string location)
        {
            return await RoutesAsync(location, (l) => l.Route.Flag == RouteFlag.Medical || l.Route.Flag == RouteFlag.All);
        }

        public async Task<Route> GetPersonAsync(int id)
        {
            var route = await GetPersonContext().FirstOrDefaultAsync(l => l.Id == id);
            return LocationHelper.MapRoute(route);
        }
        public async Task<Route> GetRouteAsync(int destinationId)
        {
            var route = await GetLocationContext().FirstOrDefaultAsync(l => l.Route.Id == destinationId);
            return LocationHelper.MapRoute(route);
        }

        public async Task<IEnumerable<Route>> GetPersonAsync(string param)
        {
            param = CleanString(param);
            return (await GetPersonContext()
                .Where(l => l.Lastname.ToLowerInvariant().Contains(param)
                    || l.Firstname.ToLowerInvariant().Contains(param)
                    || string.Concat(l.Firstname, l.Lastname).ToLowerInvariant().Contains(param)
                    || string.Concat(l.Lastname, l.Firstname).ToLowerInvariant().Contains(param))
                .ToListAsync())
                .Select(LocationHelper.MapRoute);
        }

        private IIncludableQueryable<Infrastructure.Database.Models.Person, Infrastructure.Database.Models.Route> GetPersonContext()
        {
            return _context
                .People
                .Include(p => p.Location)
                .Include(p => p.Location.Route);
        }

        private IIncludableQueryable<Location, Infrastructure.Database.Models.Route> GetLocationContext()
        {
            return _context
                .Locations
                .Include(c => c.Route);
        }

        private async Task<IEnumerable<Route>> RoutesAsync(string location, Func<Location, bool> funct)
        {
            return (await GetLocationContext()
             .Where(l => l.Name.Contains(location) && funct(l))
             .ToListAsync())
             .OrderBy(c => GetDistance(c.Name.Trim(), location))
             .Select(LocationHelper.MapRoute);
        }

        private string CleanString(string param)
        {
            return param.Replace(" ", "");
        }

        private decimal GetDistance(string p1, string searchFilter)
        {
            return string.IsNullOrEmpty(searchFilter)
            ? 0
            : (Math.Abs(p1.Length - searchFilter.Length)) / searchFilter.Length;
        }

    }
}
