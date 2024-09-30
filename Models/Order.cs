namespace BusTrackingSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public int AdultTickets { get; set; }
        public int ChildTickets { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string QrCodeUrl { get; set; }
    }
}
