using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using BusTrackingSystem.Models;
using BusTrackingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BusTrackingSystem.Data;

namespace BusTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(IUserService userService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register User", Description = "Register a new user with a username, email, and password.", Tags = new[] { "User Management" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid data" });
            }

            var result = await _userService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return Ok(new SuccessResponse { Message = "User registered successfully" });
            }

            return BadRequest(new ErrorResponse { Message = "User registration failed", Errors = result.Errors });
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login User", Description = "Login a user with username and password.", Tags = new[] { "User Management" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid data" });
            }

            var token = await _userService.LoginUserAsync(model);
            if (token != null)
            {
                return Ok(new TokenResponse { Token = token });
            }

            return Unauthorized(new ErrorResponse { Message = "Invalid username or password" });
        }

        [HttpPost("purchaseTicket")]
        [SwaggerOperation(Summary = "Purchase Bus Ticket", Description = "Purchase a bus ticket and deduct the amount from the user's balance.", Tags = new[] { "Bus Management" })]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BalanceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PaymentResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> PurchaseTicket(string username, decimal ticketPrice)
        {
            try
            {                
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return BadRequest(new PaymentResponse { Success = false, Message = "User not found" });
                }
                
                if (user.Balance >= ticketPrice)
                {                    
                    user.Balance -= ticketPrice;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    return Ok(new BalanceResponse { Balance = user.Balance, Message = "Ticket purchased successfully!" });
                }
                else
                {
                    return BadRequest(new PaymentResponse { Success = false, Message = "Insufficient balance" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = $"An error occurred: {ex.Message}" });
            }
        }




        [HttpGet("balance/{username}")]
        public async Task<IActionResult> GetUserBalance(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(new { Balance = user.Balance });
        }

    }
}
