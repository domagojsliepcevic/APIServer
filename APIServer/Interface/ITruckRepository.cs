// ITruckRepository.cs
using APIServer.Models;

namespace APIServer.Interface
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetAllTrucks();
        Task<Truck> GetTruckById(int id);
        Task AddTruck(Truck truck);
        Task UpdateTruck(Truck truck);
        Task DeleteTruck(int id);
    }
}

