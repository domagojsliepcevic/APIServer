using System.Collections.Generic;
using SOAPServer.Models;

namespace SOAPServer.Interfaces
{
    public interface ITruckRepository
    {
        IEnumerable<Truck> GetAllTrucks();
        IEnumerable<Truck> GetTrucksBySearchTerm(string searchTerm);
        Truck GetTruckById(int id);
        void AddTruck(Truck truck);
        void UpdateTruck(int id, Truck truck);
        void DeleteTruck(int id);
    }
}
