using PepperMap.Infrastructure.Database.Models;
using Route = PepperMap.DomainLayer.Models.Route;

namespace PepperMap.DomainLayer.Mappers
{
    internal class RouteHelper
    {
        public static Route MapRoute(Location location)
        {
            if (location == null) return null;
            return new Route
            {
                DestinationName = location.Name.Trim(),
                LocationId = location.Id,
                RouteIndication = location.Route.Indicator.Trim(),
                RouteNumber = location.Route.Number.Trim()
            };
        }
    }
}
