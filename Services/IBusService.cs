using System.Collections.Generic;
using System.Threading.Tasks;
using BusTrackingSystem.Models;

namespace BusTrackingSystem.Services
{
    public interface IBusService
    {
        Task<IEnumerable<Bus>> GetBusesAsync();
        //Task<Bus> AddBusAsync(Bus bus);
    }
}
