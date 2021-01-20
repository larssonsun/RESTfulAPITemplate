using System.Threading.Tasks;

namespace RESTfulAPITemplate.Core.SeedWork
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
        Task<(bool Succeed, bool IsNoEffect)> SaveUnableNoEffectAsync();
    }
}