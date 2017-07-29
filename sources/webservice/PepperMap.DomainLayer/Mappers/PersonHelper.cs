using PepperMap.Infrastructure.Database.Models;
using Person = PepperMap.DomainLayer.Models.Person;
using PersonDb = PepperMap.Infrastructure.Database.Models.Person;

namespace PepperMap.DomainLayer.Mappers
{
    internal class PersonHelper
    {
        public static Person MapPerson(PersonDb person)
        {
            if (person == null) return null;
            return new Person
            {
                Id = person.Id,
                Firstname = person.Firstname.Trim(),
                Lastname = person.Lastname.Trim(),
                LocationId = person.LocationId,
                Service = person.Service.Trim(),
                Title = person.Title.Trim(),
                IsPatient = person.Flag == PersonType.Patient,
                IsStaff = person.Flag == PersonType.Staff,
            };
        }

    }
}
