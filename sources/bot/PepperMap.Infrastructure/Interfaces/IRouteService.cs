using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.Infrastructure.Models;

namespace PepperMap.Infrastructure.Interfaces
{
    public interface IRouteService
    {
        Task<IEnumerable<Route>> GetPublicRoutesAsync(string destination);
        Task<IEnumerable<Route>> GetMedicalRoutesAsync(string destination);
        Task<IEnumerable<Route>> GetPeopleRoutesAsync(string name);
        Task<Route> GetRouteByNumber(string routeNumber);
    }
}