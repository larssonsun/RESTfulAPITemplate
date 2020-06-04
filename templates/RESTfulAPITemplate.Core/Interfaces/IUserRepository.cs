using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IUserRepository
    {
        Task<(bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result)> IsValidAsync(LoginRequest loginRequest);
    }
}