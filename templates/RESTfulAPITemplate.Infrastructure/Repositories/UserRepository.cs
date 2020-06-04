using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<(bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result)> IsValidAsync(LoginRequest loginRequest)
        {
            // some validation logic ..
            LoginResultDTO result = null;
            result = new LoginResultDTO()
            {
                UserId = "userId",
                UserName = "userName"
            };

            var dict = new Dictionary<string, string>();
            dict.Add("RESTfulAPITemplateUserName", loginRequest.Username);

            await Task.Run(() => { return false; });

            return (true, dict, result);
        }
    }
}