using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.DomainLayer.Models;

namespace PepperMap.DomainLayer.Interfaces
{
    public interface IRouteService
    {
        Task<Route> GetRouteAsync(int destinationId);
        Task<IEnumerable<Route>> GetRoutesAsync(string location);
    }
}