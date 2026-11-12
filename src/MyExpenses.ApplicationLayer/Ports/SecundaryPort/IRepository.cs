namespace MyExpenses.ApplicationLayer.Ports.SecundaryPort;

public interface IRepository<T>
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
