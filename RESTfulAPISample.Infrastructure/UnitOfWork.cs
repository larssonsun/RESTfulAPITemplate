using System.Threading.Tasks;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DemoContext _myContext;

        public UnitOfWork(DemoContext myContext)
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
    }
}