using CarRental.src.Models;

public interface ICarRepository {
    Task<List<Car>> GetByModel(string carModel);
    Task<Car?> GetById(Guid carId);
    Task<List<CarModel>> GetAllModels();
}