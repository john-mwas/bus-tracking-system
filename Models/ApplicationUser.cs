using Microsoft.AspNetCore.Identity;

namespace BusTrackingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CreditCardNumber { get; set; }
        public decimal Balance { get; set; } = 0;
    }
}