using CarRental.src.Models;

interface ILocationRepository {
    Task<Location?> GetByCode(string locationCode);
}