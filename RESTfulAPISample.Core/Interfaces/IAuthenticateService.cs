using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.DTO;

namespace RESTfulAPISample.Core.Interface
{
    public interface IAuthenticateService
    {
        (bool IsAuthenticated, LoginResultDTO Token) IsAuthenticated(LoginRequest request);
    }
}