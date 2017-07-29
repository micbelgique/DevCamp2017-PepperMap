using PepperMap.Infrastructure.Database.Models;
using Route = PepperMap.DomainLayer.Models.Route;

namespace PepperMap.DomainLayer.Mappers
{
    internal class RouteHelper
    {
        public static Route MapRoute(Infrastructure.Database.Models.Route route)
        {
            if (route == null) return null;
            return new Route
            {
                DestinationName = $"Route {route.Number.Trim()}",
                RouteIndication = route.Indicator.Trim(),
                RouteNumber = route.Number.Trim()
            };
        }

        public static Route MapRoute(Person person)
        {
            if (person == null) return null;
            return new Route
            {
                DestinationName = person.ToString(),
                LocationId = person.LocationId,
                RouteIndication = person.Location.Route.Indicator.Trim(),
                RouteNumber = person.Location.Route.Number.Trim()
            };
        }
    }
}
