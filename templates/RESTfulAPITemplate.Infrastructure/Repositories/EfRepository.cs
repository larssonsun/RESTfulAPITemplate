using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.SeedWork;
#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Interface;
using Larsson.RESTfulAPIHelper.SortAndQuery;
#endif

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity<Guid>
    {
        protected readonly ProductContext _dbContext;
#if (RESTFULAPIHELPER)
        private readonly IPropertyMappingContainer _propertyMappingContainer;

#endif

#if (RESTFULAPIHELPER)
        public EfRepository(ProductContext dbContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _dbContext = dbContext;
            _propertyMappingContainer = propertyMappingContainer;
        }
#else
        public EfRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }
#endif


        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);

            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = new SpecificationEvaluator<T>();
            return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        public async Task<(bool has, T entity)> TryGetByIdAsNoTrackingAsync(Guid id)
        {
            var result = await _dbContext.Set<T>().AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return (result != null, result);
        }

        public async Task<(bool has, IEnumerable<T> entities)> TryListAsNoTrackingAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            var result = await specificationResult.AsNoTracking().ToListAsync();
            return (result == null ? false : result.Count() > 0, result);
        }

        public async Task<IReadOnlyList<T>> ListWithOrderAsync<VM>(ISpecification<T> spec, string orderBy)
        {
            var query = ApplySpecification(spec);

#if (RESTFULAPIHELPER)

            query = query.ApplySort(orderBy, _propertyMappingContainer.Resolve<VM, T>());

#endif

            return await query.AsNoTracking().ToListAsync();
        }
    }
}