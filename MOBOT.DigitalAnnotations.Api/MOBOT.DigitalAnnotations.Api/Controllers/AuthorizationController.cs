
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Api.Models;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    [Route("api/Authorization")]
    [ApiController]
    public class AuthorizationController : DigitalAnnotationsControllerBase<AuthorizationController>
    {
        private readonly IAuthorizationService _service;
        public AuthorizationController(IAuthorizationService service, ILogger<AuthorizationController> logger): base(logger)
        {
            _service = service;
        }

        [HttpPost(), Route("Login")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody]LoginRequestModel request) {
            if (ModelState.IsValid)
            {
                var model = await _service.LoginAsync(request.UserName, request.Password);
                if (model != null)
                    return Ok(model);
            }

            return BadRequest("Invalid login credentials.");
        }
    }
}