using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    /// <summary>
    /// Controller to handle Web Annotation Requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebAnnotationsController : DigitalAnnotationsControllerBase<WebAnnotationsController>
    {
        private readonly IWebAnnotationService _service;
        

        public WebAnnotationsController(IWebAnnotationService annotationService, ILogger<WebAnnotationsController> logger) : base(logger)
        {
            _service = annotationService;
        }

        /// <summary>
        /// Gets the web annotation for the given annotation id.
        /// </summary>
        /// <param name="id">The id of the annotation resource.</param>
        /// <returns></returns>
        [HttpGet(), Route("{id}")]
        [ProducesResponseType(typeof(WebAnnotation), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnnotationById(int id)
        {
            _logger.LogDebug($"Get Web Annotation called using id: {id}.");
            try
            {
                var wa = await _service.GetAnnotationById(id);                
                return Ok(wa);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred retrieving web annotation for id: {id}.");
            }
        }
    }
}