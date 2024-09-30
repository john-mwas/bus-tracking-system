using BusTrackingSystem.Data;
using BusTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusTrackingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task CreateOrderAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while adding the order.");
                throw new ApplicationException("Database error occurred while adding the order. Please try again later.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the order.");
                throw new ApplicationException("An unexpected error occurred. Please try again.", ex);
            }
        }

        public async Task<List<Order>> GetOrdersByUsernameAsync(string username)
        {
            try
            {
                return await _context.Orders
                    .Where(o => o.Username == username)
                    .ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Invalid operation when fetching orders for user: {username}");
                throw new ApplicationException("An error occurred while fetching the orders. Please try again.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while fetching orders for user: {username}");
                throw new ApplicationException("An unexpected error occurred. Please try again.", ex);
            }
        }
        
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.SingleOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
                }

                return order;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Invalid operation when fetching order with ID: {orderId}");
                throw new ApplicationException("An error occurred while fetching the order. Please try again.", ex);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                throw;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while fetching the order with ID: {orderId}");
                throw new ApplicationException("An unexpected error occurred. Please try again.", ex);
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} was not found and cannot be deleted.");
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database error occurred while deleting the order with ID: {orderId}");
                throw new ApplicationException("Database error occurred while deleting the order. Please try again later.", dbEx);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                throw;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting the order with ID: {orderId}");
                throw new ApplicationException("An unexpected error occurred. Please try again.", ex);
            }
        }
    }
}
