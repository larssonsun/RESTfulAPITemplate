using System;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IScetiaUserRepository
    {
        Task<AspnetMembership> GetMembershipByUserId(Guid userId);
        Task<AspnetUsers> GetUserByName(string userName);
    }
}