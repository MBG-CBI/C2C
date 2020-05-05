using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Business.Interfaces;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebManifestsController : DigitalAnnotationsControllerBase<WebManifestsController>
    {
        private readonly IWebManifestService _webManifestService;

        public WebManifestsController(IWebManifestService webManifestService, ILogger<WebManifestsController> logger): base(logger)
        {
            _webManifestService = webManifestService;
        }

        [HttpGet(), Route("{id}")]
        public async Task<IActionResult> GetByAnnotatonSourceId(long id)
        {
            var model = await _webManifestService.GetByAnnotationSourceId(id);
            return Ok(model);
        }
    }
}