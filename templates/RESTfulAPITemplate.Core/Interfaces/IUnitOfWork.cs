using System.Threading.Tasks;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
        Task<(bool Succeed, bool IsNoEffect)> SaveUnableNoEffectAsync();
    }
}