using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IAsyncRepository<T> where T : BaseEntity<Guid>
    {
        T Add(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
        void Delete(T entity);
        Task<T> FirstAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListWithOrderAsync<VM>(ISpecification<T> spec, string orderBy);
        Task<(bool has, T entity)> TryGetByIdAsNoTrackingAsync(Guid id);
        Task<(bool has, IEnumerable<T> entities)> TryListAsNoTrackingAsync(ISpecification<T> spec);
        void Update(T entity);
    }
}