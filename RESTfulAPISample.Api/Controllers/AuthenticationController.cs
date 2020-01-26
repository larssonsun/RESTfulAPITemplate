using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPISample.Api.Resource;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Api.Controller
{
    /// <summary>
    /// Authentication
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _auth;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticateService auth, IMapper mapper)
        {
            this._auth = auth;
            this._mapper = mapper;
        }
        
        /// <summary>
        /// Get JWT Token
        /// </summary>
        /// <param name="request">User login info</param>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="400">If authorization verification is not passed</response>
        /// <response code="422">DTO LoginRequestDTO failed to pass the model validation</response>
        [AllowAnonymous]
        [HttpPost("request-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RequestToken([FromBody] LoginRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
            }

            var loginRequest = _mapper.Map<LoginRequest>(request);

            var (isAuth, token) = _auth.IsAuthenticated(loginRequest);
            if (isAuth)
            {
                return Ok(token);
            }

            return BadRequest("Invalid Request");
        }
    }
}