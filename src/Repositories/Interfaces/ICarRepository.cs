using CarRental.src.Models;

interface ICarRepository {
    Task<List<Car>> GetByModel(string carModel);
    Task<Car?> GetById(Guid carId);
}