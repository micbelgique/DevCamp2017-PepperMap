using System.Collections.Generic;
using System.Threading.Tasks;
using PepperMap.DomainLayer.Models;

namespace PepperMap.DomainLayer.Interfaces
{
    public interface IRouteService
    {
        Task<Route> GetRouteAsync(int destinationId);
        Task<IEnumerable<Route>> GetPublicRoutesAsync(string location);
        Task<IEnumerable<Route>> GetMedicalRoutesAsync(string location);
        Task<Route> GetPersonAsync(int id);
        Task<IEnumerable<Route>> GetPersonAsync(string param);
    }
}