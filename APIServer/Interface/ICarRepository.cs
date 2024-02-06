using APIServer.Models;

namespace APIServer.Interface
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars();
        Task<Car> GetCarById(int id);
        Task AddCar(Car car);
        Task UpdateCar(Car car);
        Task DeleteCar(int id);
    }
}
