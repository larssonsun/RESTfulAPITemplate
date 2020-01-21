using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        // Implement your own validation logic pls.
        public bool IsValid(LoginRequest req)
        {
            return true;
        }
    }
}