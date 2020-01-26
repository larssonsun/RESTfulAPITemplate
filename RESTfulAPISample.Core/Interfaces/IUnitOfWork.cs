using System.Threading.Tasks;

namespace RESTfulAPISample.Core.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}