using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AnnotationFiltersController : DigitalAnnotationsControllerBase<AnnotationFiltersController>
    {
        private readonly IAnnotationFilterService _service;
        public AnnotationFiltersController(IAnnotationFilterService annotationFilterService, ILogger<AnnotationFiltersController> logger) : base(logger)
        {
            _service = annotationFilterService;
        }


        [HttpGet(), Route("Source/{sourceId}")]
        [ProducesResponseType(typeof(IEnumerable<AnnotationFilterTypeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListBySource(int sourceId) {
            try
            {
                var list = await _service.GetFiltersForSource(sourceId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred getting Annotation Filters by source. SourceId {sourceId}");
            }
        
        }
    }
}