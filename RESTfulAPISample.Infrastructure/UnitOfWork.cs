using System.Threading.Tasks;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RESTfulAPISampleContext _myContext;

        public UnitOfWork(RESTfulAPISampleContext myContext)
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