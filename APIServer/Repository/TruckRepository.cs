// TruckRepository.cs
using APIServer.Data;
using APIServer.Interface;
using APIServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Repositories
{
    public class TruckRepository : ITruckRepository
    {
        private readonly DatabaseContext _context;

        public TruckRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Truck>> GetAllTrucks()
        {
            return await _context.Trucks.ToListAsync();
        }

        public async Task<Truck> GetTruckById(int id)
        {
            return await _context.Trucks.FindAsync(id);
        }

        public async Task AddTruck(Truck truck)
        {
            _context.Trucks.Add(truck);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTruck(Truck truck)
        {
            _context.Entry(truck).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTruck(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();
            }
        }
    }
}
