using System.Collections.Generic;
using System.Linq;
using SOAPServer.Data;
using SOAPServer.Interfaces;
using SOAPServer.Models;

namespace SOAPServer.Repositories
{
    public class TruckRepository : ITruckRepository
    {
        private readonly TrucksContext _context;

        public TruckRepository(TrucksContext context)
        {
            _context = context;
        }

        public IEnumerable<Truck> GetAllTrucks()
        {
            return _context.Trucks.ToList();
        }

        public IEnumerable<Truck> GetTrucksBySearchTerm(string searchTerm)
        {
            return _context.Trucks.
                Where(c => c.Make.Contains(searchTerm) ||
                c.Model.Contains(searchTerm) ||
                c.Year.ToString().Contains(searchTerm) ||
                c.Color.Contains(searchTerm))
                .ToList();

        }

        public Truck GetTruckById(int id)
        {
            return _context.Trucks.Find(id);
        }

        public void AddTruck(Truck truck)
        {
            _context.Trucks.Add(truck);
            _context.SaveChanges();
        }

        public void UpdateTruck(int id, Truck truck)
        {
            var existingCar = _context.Trucks.Find(id);
            if (existingCar != null)
            {
                existingCar.Make = truck.Make;
                existingCar.Model = truck.Model;
                existingCar.Year = truck.Year;
                existingCar.Color = truck.Color;
                _context.SaveChanges();
            }
        }

        public void DeleteTruck(int id)
        {
            var truck = _context.Trucks.Find(id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                _context.SaveChanges();
            }
        }
    }
}
