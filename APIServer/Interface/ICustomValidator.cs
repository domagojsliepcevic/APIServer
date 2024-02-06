// ICustomValidator.cs
using APIServer.Models;

namespace APIServer.Interface
{
    public interface ICustomValidator
    {
        bool ValidateTruck(Truck truck);
    }
}
