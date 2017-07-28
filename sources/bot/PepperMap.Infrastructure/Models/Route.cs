namespace PepperMap.Infrastructure.Models
{
    public class Route
    {
        public string DestinationName { get; set; }
        public string RouteNumber { get; set; }
        public string RouteIndication { get; set; }

        public override string ToString()
        {
            return $"{RouteNumber} via {RouteIndication}";
        }
    }
}
