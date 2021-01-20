using System;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.Interface;
using RESTfulAPITemplate.Core.SeedWork;

namespace RESTfulAPITemplate.Core.Service
{
    public class ScetiaUserService : IScetiaUserService
    {
        private readonly IScetiaIndentityUtil _scetiaIndentityUtil;

        private IScetiaUserRepository _userRepository;

        public ScetiaUserService(IScetiaIndentityUtil scetiaIndentityUtil, IScetiaUserRepository userRepository)
        {
            _scetiaIndentityUtil = scetiaIndentityUtil;
            _userRepository = userRepository;
        }

        public async Task<(bool IsValid, Guid UserId)> IsUserLoginValid(string userName, string password)
        {
            var user = await _userRepository.GetUserByName(userName);
            if (user == null)
            {
                return (false, Guid.Empty);
            }

            var membership = await _userRepository.GetMembershipByUserId(user.UserId);
            if (user.UserId != membership.UserId)
            {
                return (false, Guid.Empty);
            }

            return (_scetiaIndentityUtil.ValidatePassword(membership.Password, membership.PasswordSalt, password), user.UserId);
        }
    }
}