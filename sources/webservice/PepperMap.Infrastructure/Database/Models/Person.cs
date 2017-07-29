namespace PepperMap.Infrastructure.Database.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Title { get; set; }
        public string Service { get; set; }
        public PersonType Flag { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            switch (Flag)
            {
                case PersonType.Patient:
                    return $"patient {Lastname} {Firstname}";
                case PersonType.Staff:
                    return $"medecin {Lastname} {Firstname} dans le service {Service}";
                default:
                    return $"not found";
            }
        }
    }

    public enum PersonType
    {
        Patient = 1,
        Staff = 2
    }
}
