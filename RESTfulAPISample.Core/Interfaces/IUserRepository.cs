using System.Collections.Generic;
using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IUserRepository
    {
        (bool IsValid, Dictionary<string, string> Payload) IsValid(LoginRequest loginRequest);
    }
}