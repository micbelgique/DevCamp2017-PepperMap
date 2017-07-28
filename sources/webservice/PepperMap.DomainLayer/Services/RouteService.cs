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
    public class RouteService : IRouteService
    {

        private readonly DatabaseContext _context;
        public RouteService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Route> GetRouteAsync(int destinationId)
        {
            var location = await GetLocationContext()
                .FirstOrDefaultAsync(l => l.Id == destinationId);
            return RouteHelper.MapRoute(location);
        }

        public async Task<IEnumerable<Route>> GetPublicRoutesAsync(string location)
        {
            return await RoutesAsync(location, RouteFlag.Public);
        }

        public async Task<IEnumerable<Route>> GetMedicalRoutesAsync(string location)
        {
            return await RoutesAsync(location, RouteFlag.Medical);
        }

        public async Task<Route> GetPersonAsync(int id)
        {
            var route = await GetPersonContext().FirstOrDefaultAsync(l => l.Id == id);
            return RouteHelper.MapRoute(route);
        }

        public async Task<IEnumerable<Route>> GetPersonAsync(string param)
        {
            param = CleanString(param);
            return (await GetPersonContext()
                .Where(l => l.Lastname.Contains(param)
                    || l.Firstname.Contains(param)
                    || $"{l.Firstname}{l.Lastname}".Contains(param)
                    || $"{l.Lastname}{l.Firstname}".Contains(param))
                .ToListAsync())
                .Select(RouteHelper.MapRoute);
        }

        private IIncludableQueryable<Person, Infrastructure.Database.Models.Route> GetPersonContext()
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

        private async Task<IEnumerable<Route>> RoutesAsync(string location, RouteFlag flag)
        {
            return (await GetLocationContext()
             .Where(l => l.Name.Contains(location) && l.Route.Flag == flag)
             .ToListAsync())
             .Select(RouteHelper.MapRoute);
        }

        private string CleanString(string param)
        {
            return param.Replace(" ", "");
        }
    }
}
