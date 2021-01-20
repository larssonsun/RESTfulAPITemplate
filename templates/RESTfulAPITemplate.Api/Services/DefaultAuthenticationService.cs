using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using RESTfulAPITemplate.App.Model;
using AutoMapper;
using RESTfulAPITemplate.Core.Interface;
using RESTfulAPITemplate.Core.Entity;
using System.Collections.Generic;

namespace RESTfulAPITemplate.App.Service
{
    public class DefaultAuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly TokenManagementConf _tokenManagement;

        public DefaultAuthenticationService(IMapper mapper, IOptions<TokenManagementConf> tokenManagement)
        {
            _mapper = mapper;
            _tokenManagement = tokenManagement.Value;
        }

        public async Task<(bool IsAuthenticated, LoginResultDTO Token)> IsAuthenticated(LoginCommandDTO request)
        {
            var loginCommand = _mapper.Map<LoginCommand>(request);

            // Some login verification logic ..
            var loginResult = await Task.FromResult<(bool IsValid, Guid UserId)>((true, Guid.NewGuid()));

            if (!loginResult.IsValid)
                return (false, null);

            LoginResultDTO loginResultDTO = null;
            loginResultDTO = new LoginResultDTO()
            {
                UserId = loginResult.UserId.ToString(),
                UserName = request.Username
            };

            var dict = new Dictionary<string, string>();
            dict.Add("UserName", loginResultDTO.UserName);
            dict.Add("UserId", loginResultDTO.UserId);

            var claims = dict?.Select(x => new Claim(x.Key, x.Value));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: _tokenManagement.Issuer,
                audience: _tokenManagement.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            loginResultDTO.Token = token;

            return (true, loginResultDTO);
        }
    }
}