using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PepperMap.DomainLayer.Models;

namespace PepperMap.DomainLayer.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPatientAsync();
        Task<IEnumerable<Person>> GetPatientAsync(string filter);
        Task<IEnumerable<Person>> GetStaffAsync();
        Task<IEnumerable<Person>> GetStaffAsync(string filter);
    }
}
