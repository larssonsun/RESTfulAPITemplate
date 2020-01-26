using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IAuthenticateService
    {
        (bool IsAuthenticated, string Token) IsAuthenticated(LoginRequest request);
    }
}