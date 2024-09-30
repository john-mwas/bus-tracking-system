using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusTrackingSystem.Models;

namespace BusTrackingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email, CreditCardNumber = model.CreditCardNumber };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await InitializeBalanceAsync(user, 100);
                _logger.LogInformation("User {Username} registered successfully.", model.Username);
            }
            else
            {
                _logger.LogWarning("User {Username} registration failed: {Errors}", model.Username, string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

        public async Task<string> LoginUserAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
                if (appUser == null)
                {
                    _logger.LogWarning("Login failed: User {Username} not found.", model.Username);
                    return null;
                }
                _logger.LogInformation("User {Username} logged in successfully.", model.Username);
                return GenerateJwtToken(model.Username, appUser);
            }

            _logger.LogWarning("Login failed for user {Username}: {Result}", model.Username, result.ToString());
            return null;
        }

        private string GenerateJwtToken(string username, ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task InitializeBalanceAsync(ApplicationUser user, decimal amount)
        {
            user.Balance = amount;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> MakePaymentAsync(ApplicationUser user, decimal amount)
        {
            if (user.Balance >= amount)
            {
                user.Balance -= amount;
                await _userManager.UpdateAsync(user);
                return true;
            }
            return false;
        }

    }
}
