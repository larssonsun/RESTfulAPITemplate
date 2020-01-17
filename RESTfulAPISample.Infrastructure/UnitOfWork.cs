using System.Threading.Tasks;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;

        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<int> SaveAsync()
        {
            return await _myContext.SaveChangesAsync();
        }
    }
}