using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.Infrastructure.Models;

namespace PepperMap.Infrastructure.Interfaces
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> GetRoutesAsync(string destination);
    }
}