using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusTrackingSystem.Data;
using BusTrackingSystem.Models;

namespace BusTrackingSystem.Services
{
    public class BusService : IBusService
    {
        private readonly ApplicationDbContext _context;

        public BusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetBusesAsync()
        {
            return await _context.Buses.ToListAsync();
        }

        //public async Task<Bus> AddBusAsync(Bus bus)
        //{
        //    _context.Buses.Add(bus);
        //    await _context.SaveChangesAsync();
        //    return bus;
        //}
    }
}
