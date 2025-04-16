using CarRental.src.Models;

public interface ILocationRepository {
    Task<Location?> GetByCode(string locationCode);
}