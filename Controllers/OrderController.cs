using BusTrackingSystem.Models;
using BusTrackingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusTrackingSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BusTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public OrderController(IOrderService orderService, ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _orderService = orderService;
            _context = context;
            _userManager = userManager;
            _env = env;
        }


        [HttpGet("generateOrderPage/{orderId}")]
        [SwaggerOperation(Summary = "Generate URL", Description = "Generate url for order.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GenerateOrderPage(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            
            var templatePath = Path.Combine(_env.WebRootPath, "OrderTemplate.html");
            var htmlContent = await System.IO.File.ReadAllTextAsync(templatePath);
            
            htmlContent = htmlContent.Replace("{{OrderId}}", order.OrderId.ToString());
            htmlContent = htmlContent.Replace("{{Username}}", order.Username);
            htmlContent = htmlContent.Replace("{{AdultTickets}}", order.AdultTickets.ToString());
            htmlContent = htmlContent.Replace("{{ChildTickets}}", order.ChildTickets.ToString());
            htmlContent = htmlContent.Replace("{{ExpiryDate}}", order.ExpiryDate.ToString("f"));


            var fileName = $"Order_{order.OrderId}.html";
            var filePath = Path.Combine(_env.WebRootPath, fileName);

            await System.IO.File.WriteAllTextAsync(filePath, htmlContent);


            var fileUrl = $"{Request.Scheme}://{Request.Host}/{fileName}";
            order.QrCodeUrl = fileUrl; 

            await _orderService.UpdateOrderAsync(order); 

            return Ok(new { Url = fileUrl });
        }

        [HttpPost("createOrder")]
        [SwaggerOperation(Summary = "Create Order", Description = "Create a new order for bus tickets.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateOrder(string username, int adultTickets, int childTickets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid data" });
            }

            try
            {
                var model = new Order() { Username = username, AdultTickets = adultTickets,
                    ChildTickets = childTickets };
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.Username);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse { Message = "User not found" });
                }

                model.ExpiryDate = DateTime.Now.AddHours(24);
                model.QrCodeUrl = "";                

                await _orderService.CreateOrderAsync(model);              
                

                return Ok(new SuccessResponse { Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = $"An error occurred: {ex.Message}" });
            }
        }

        
        [HttpGet("getOrders")]
        [SwaggerOperation(Summary = "Get Orders", Description = "Get all orders for a specific user.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetOrders(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return NotFound(new ErrorResponse { Message = "User not found" });
                }

                var orders = await _orderService.GetOrdersByUsernameAsync(username);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = $"An error occurred: {ex.Message}" });
            }
        }

       
        [HttpGet("getOrderById")]
        [SwaggerOperation(Summary = "Get Order", Description = "Get a specific order by ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound(new ErrorResponse { Message = "Order not found" });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = $"An error occurred: {ex.Message}" });
            }
        }

      
        [HttpDelete("deleteOrderById")]
        [SwaggerOperation(Summary = "Delete Order", Description = "Delete a specific order by ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound(new ErrorResponse { Message = "Order not found" });
                }

                await _orderService.DeleteOrderAsync(orderId);
                return Ok(new SuccessResponse { Message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
