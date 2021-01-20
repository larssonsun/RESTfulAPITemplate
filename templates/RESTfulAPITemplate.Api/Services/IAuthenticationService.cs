using System.Threading.Tasks;
using RESTfulAPITemplate.App.Model;

namespace RESTfulAPITemplate.App.Service
{
    public interface IAuthenticationService
    {
        Task<(bool IsAuthenticated, LoginResultDTO Token)> IsAuthenticated(LoginCommandDTO request);
    }
}