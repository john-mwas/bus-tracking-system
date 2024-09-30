using BusTrackingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusTrackingSystem.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByUsernameAsync(string username);
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task DeleteOrderAsync(int orderId); 
        Task UpdateOrderAsync(Order order);
    }
}
