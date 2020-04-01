using System.Collections.Generic;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        public (bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result) IsValid(LoginRequest loginRequest)
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

            return (true, dict, result);
        }
    }
}