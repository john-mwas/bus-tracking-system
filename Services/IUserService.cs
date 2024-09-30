using System.Threading.Tasks;
using BusTrackingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace BusTrackingSystem.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<string> LoginUserAsync(LoginModel model);
        Task InitializeBalanceAsync(ApplicationUser user, decimal amount);
        Task<bool> MakePaymentAsync(ApplicationUser user, decimal amount);
    }
}
