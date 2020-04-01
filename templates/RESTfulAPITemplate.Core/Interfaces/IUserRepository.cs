using System.Collections.Generic;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IUserRepository
    {
        (bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result) IsValid(LoginRequest loginRequest);
    }
}