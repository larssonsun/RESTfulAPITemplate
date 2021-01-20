using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPITemplate.App.Model;
using RESTfulAPITemplate.Core.Specification.Filter;
using RESTfulAPITemplate.App.Service;

namespace RESTfulAPITemplate.App.Controller
{
    /// <summary>
    /// Authentication
    /// </summary>
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _auth;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticationService auth, IMapper mapper)
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
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> RequestToken([FromBody] LoginCommandDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            // larsson：这里必须startup中设置禁用自动400响应，SuppressModelStateInvalidFilter = true。否则Model验证失败后这里的ProductResource永远是null
            if (!ModelState.IsValid)
            {
                // larsson：如果要自定义422之外的响应则需要新建一个类继承UnprocessableEntityObjectResult
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var (isAuth, result) = await _auth.IsAuthenticated(request);
            if (isAuth)
            {
                return Ok(result);
            }

            return BadRequest("wrong user name or password");
        }
    }
}