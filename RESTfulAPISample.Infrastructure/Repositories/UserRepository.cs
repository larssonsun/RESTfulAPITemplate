using System.Collections.Generic;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.DTO;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
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
            dict.Add("RESTfulAPISampleUserName", loginRequest.Username);

            return (true, dict, result);
        }
    }
}