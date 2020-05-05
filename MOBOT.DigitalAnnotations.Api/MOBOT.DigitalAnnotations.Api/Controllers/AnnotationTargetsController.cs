using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    /// <summary>
    /// API Controller to handle AnnotationTarget resource requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationTargetsController : DigitalAnnotationsControllerBase<AnnotationTargetsController>
    {
        private readonly IAnnotationTargetService _service;
      
        /// <summary>
        /// Constructor to allow dependency injection.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public AnnotationTargetsController(IAnnotationTargetService service, ILogger<AnnotationTargetsController> logger) : base(logger)
        {
            _service = service;
        }

        /// <summary>
        /// Gets a list of Annotation resources from the underlying data using the supplied target id.
        /// </summary>
        /// <param name="id">The id of the AnnotationTarget resource in which the Annotation resources belong.</param>
        /// <returns></returns>
        [HttpGet(), Route("{id}/Annotations")]
        [ProducesResponseType(typeof(IEnumerable<AnnotationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnnotations(long id)
        {

            _logger.LogDebug($"Get Annotations called with target id: {id}.");
            
            try
            {
                var annotations = await _service.GetAnnotations(id);
                return Ok(annotations);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred retrieving annotations for target id: {id}.");
            }
        }
    }
}