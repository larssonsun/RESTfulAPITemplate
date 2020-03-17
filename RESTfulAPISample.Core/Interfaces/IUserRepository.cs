using System.Collections.Generic;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.DTO;

namespace RESTfulAPISample.Core.Interface
{
    public interface IUserRepository
    {
        (bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result) IsValid(LoginRequest loginRequest);
    }
}