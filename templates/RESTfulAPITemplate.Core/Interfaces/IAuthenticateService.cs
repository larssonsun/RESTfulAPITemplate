using System.Threading.Tasks;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IAuthenticateService
    {
        Task<(bool IsAuthenticated, LoginResultDTO Token)> IsAuthenticated(LoginRequest request);
    }
}