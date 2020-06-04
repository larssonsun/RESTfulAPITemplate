using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.Interface;
using RESTfulAPITemplate.Core.DTO;
using System.Threading.Tasks;

namespace RESTfulAPITemplate.Api.Service
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserRepository _userService;
        private readonly TokenManagement _tokenManagement;
        public TokenAuthenticationService(IUserRepository userService, IOptions<TokenManagement> tokenManagement)
        {
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
        }
        public async Task<(bool IsAuthenticated, LoginResultDTO Token)> IsAuthenticated(LoginRequest request)
        {

            var validateResult = await _userService.IsValidAsync(request);
            if (!validateResult.IsValid)
                return (false, null);

            var claims = validateResult.Payload?.Select(x => new Claim(x.Key, x.Value));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: _tokenManagement.Issuer,
                audience: _tokenManagement.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            validateResult.result.Token = token;

            return (true, validateResult.result);

        }
    }
}