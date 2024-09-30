namespace BusTrackingSystem.Models
{
    public class Bus
    {
        public string Id { get; set; }
        public string Line { get; set; }
        public string Direction { get; set; }
        public string Operator { get; set; }
        public string EstimatedArrival { get; set; }
    }
}
