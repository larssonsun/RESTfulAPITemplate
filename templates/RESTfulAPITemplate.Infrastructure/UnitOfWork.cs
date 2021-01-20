using System.Threading.Tasks;
using RESTfulAPITemplate.Core.SeedWork;

namespace RESTfulAPITemplate.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _myContext;

        public UnitOfWork(ProductContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                return await _myContext.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }

        }

        public async Task<(bool Succeed, bool IsNoEffect)> SaveUnableNoEffectAsync()
        {
            try
            {
                var i = await _myContext.SaveChangesAsync();
                return (true, i == 0);
            }
            catch
            {
                return (false, false);
            }
        }
    }
}