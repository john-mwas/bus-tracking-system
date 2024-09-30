using Microsoft.AspNetCore.Identity;

namespace BusTrackingSystem.Models
{
    public class SuccessResponse
    {
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }

    public class BalanceResponse
    {
        public decimal Balance { get; set; }
        public string Message { get; set; }
    }

    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
