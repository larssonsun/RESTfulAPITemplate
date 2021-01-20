using System;
using System.Threading.Tasks;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IScetiaUserService
    {
        Task<(bool IsValid, Guid UserId)> IsUserLoginValid(string userName, string password);
    }
}