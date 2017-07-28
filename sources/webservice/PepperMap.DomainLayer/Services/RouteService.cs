using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Route>> GetRoutesAsync(string location)
        {
            return (await GetLocationContext()
                .Where(l => l.Name.Contains(location))
                .ToListAsync())
                .Select(RouteHelper.MapRoute);
        }
        private IIncludableQueryable<Location, Infrastructure.Database.Models.Route> GetLocationContext()
        {
            return _context
                .Locations
                .Include(c => c.Route);
        }

    }
}
