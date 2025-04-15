using CarRental.src.Models;

interface ICarRepository {
    Task<List<Car>> GetByModel(CarModel carModel);
    Task<Car?> GetById(Guid carId);
}