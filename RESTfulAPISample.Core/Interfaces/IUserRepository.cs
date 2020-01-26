using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IUserRepository
    {
        bool IsValid(LoginRequest req);
    }
}