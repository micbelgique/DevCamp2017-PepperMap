using System;
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
            return LocationHelper.MapRoute(location);
        }

        public async Task<Route> GetRouteByNumberAsync(string routeNumber)
        {
            var routeDb = await GetRouteContext()
                .FirstOrDefaultAsync(l => string.Compare(l.Number.Trim(), routeNumber, StringComparison.CurrentCultureIgnoreCase) == 0);
            return RouteHelper.MapRoute(routeDb);
        }

        private IIncludableQueryable<Location, Infrastructure.Database.Models.Route> GetLocationContext()
        {
            return _context
                .Locations
                .Include(c => c.Route);
        }

        private DbSet<Infrastructure.Database.Models.Route> GetRouteContext()
        {
            return _context.Routes;
        }
    }
}