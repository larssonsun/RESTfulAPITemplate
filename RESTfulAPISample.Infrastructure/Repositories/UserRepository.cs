using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        //模拟测试，默认都是人为验证有效
        public bool IsValid(LoginRequest req)
        {
            return true;
        }
    }
}