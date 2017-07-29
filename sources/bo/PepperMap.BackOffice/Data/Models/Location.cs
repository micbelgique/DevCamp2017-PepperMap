namespace PepperMap.BackOffice.Data.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string Name { get; set; }
        public Route Route { get; set; }

    }
}
