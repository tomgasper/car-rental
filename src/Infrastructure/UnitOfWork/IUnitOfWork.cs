namespace CarRental.src.Infrastructure.UnitOfWork;
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}