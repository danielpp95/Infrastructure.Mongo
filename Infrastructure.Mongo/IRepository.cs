namespace Infrastructure.Mongo;

using System.Linq.Expressions;

public interface IRepository<T> where T : IEntity
{
    Task<T> FindByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, CancellationToken token = default);

    Task SaveAsync(T entity, CancellationToken token = default);

    Task SaveAsync(IReadOnlyCollection<T> entities, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}
