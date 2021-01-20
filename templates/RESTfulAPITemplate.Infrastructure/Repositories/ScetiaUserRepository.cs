using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class ScetiaUserRepository : IScetiaUserRepository
    {
        private readonly ScetiaIndentityContext _context;


        public ScetiaUserRepository(ScetiaIndentityContext context)
        {
            _context = context;
        }

        public async Task<AspnetUsers> GetUserByName(string userName)
        {
            return await _context.AspnetUsers
                .Where(x => x.UserName.ToLower() == userName.ToLower())
                .SingleOrDefaultAsync();
        }

        public async Task<AspnetMembership> GetMembershipByUserId(Guid userId)
        {
            return await _context.AspnetMembership
                .Where(x => x.UserId == userId)
                .SingleOrDefaultAsync();
        }
    }
}