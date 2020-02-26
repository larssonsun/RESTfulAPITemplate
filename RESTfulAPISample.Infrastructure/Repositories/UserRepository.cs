using System.Collections.Generic;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {        
        public (bool IsValid, Dictionary<string, string> Payload) IsValid(LoginRequest loginRequest)
        {
            // some validation logic ..

            var dict = new Dictionary<string, string>();
            dict.Add("RESTfulAPISampleUserName", loginRequest.Username);

            return (true, dict);
        }
    }
}