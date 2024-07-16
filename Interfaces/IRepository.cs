using System.Linq.Expressions;

public interface IRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> AddAsync(T entity);
    Task<IEnumerable<T>?> AddRangeAsync(IEnumerable<T> entities);
    Task<T?> UpdateAsync(T entity);
    Task<bool> RemoveAsync(T entity);

    // Task<IEnumerable<T>?> GetAllIncludeAsync(params Expression<Func<T, object>>[] includes);
    // Task UpdateAsync(T entity);
    // Task DeleteAsync(T entity);
    // Task SaveChangesAsync();
}
