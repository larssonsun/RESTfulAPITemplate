using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(LoginRequest request, out string token);
    }
}