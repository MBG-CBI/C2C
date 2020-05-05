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
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationTypeController : DigitalAnnotationsControllerBase<AnnotationTypeController>
    {
        private readonly IAnnotationTypeService _service;

        public AnnotationTypeController(IAnnotationTypeService annotationTypeService, ILogger<AnnotationTypeController> logger) : base(logger)
        {
            _service = annotationTypeService;
        }

        /// <summary>
        /// Gets a list of Annotation Type Resources.
        /// </summary>
        /// <returns></returns>
        [HttpGet(), Route("List")]
        [ProducesResponseType(typeof(IEnumerable<AnnotationTypeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList() {
            try
            {
                _logger.LogDebug("Get Annotation Type List called.");
                var types = await _service.GetListAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, "An error occurred retrieving Annotation Type List.");
            }
        }
    }
}