using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class ScetiaUserRepository : IUserRepository
    {
        private readonly ScetiaIndentityContext _context;
        private readonly IScetiaIndentityUtil _scetiaIndentityUtil;

        public ScetiaUserRepository(ScetiaIndentityContext context, IScetiaIndentityUtil scetiaIndentityUtil)
        {
            _context = context;
            _scetiaIndentityUtil = scetiaIndentityUtil;
        }

        public async Task<(bool IsValid, Dictionary<string, string> Payload, LoginResultDTO result)> IsValidAsync(LoginRequest loginRequest)
        {
            var user = await _context.AspnetUsers
                .Where(x => x.UserName.ToLower() == loginRequest.Username.ToLower()).SingleOrDefaultAsync();

            if (user == null)
                return (false, null, null);

            var membership = await _context.AspnetMembership
                .Where(x => x.UserId == user.UserId)
                .SingleOrDefaultAsync();

            if (membership == null)
                return (false, null, null);

            var isValid = _scetiaIndentityUtil.ValidatePassword(membership.Password, membership.PasswordSalt, loginRequest.Password);
            if(!isValid)
                return (false, null, null);

            LoginResultDTO result = null;
            result = new LoginResultDTO()
            {
                UserId = user.UserId.ToString(),
                UserName = user.UserName
            };

            var dict = new Dictionary<string, string>();
            dict.Add("UserName", loginRequest.Username);
            dict.Add("UserId", user.UserId.ToString());

            return (isValid, dict, result);
        }
    }
}