using System.Threading.Tasks;

namespace RESTfulAPISample.Core.Interface
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}