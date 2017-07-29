using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.DomainLayer.Models;

namespace PepperMap.DomainLayer.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Route>> GetMedicalRoutesAsync(string location);
        Task<Route> GetPersonAsync(int id);
        Task<IEnumerable<Route>> GetPersonAsync(string param);
        Task<IEnumerable<Route>> GetPublicRoutesAsync(string location);
        Task<Route> GetRouteAsync(int destinationId);
    }
}