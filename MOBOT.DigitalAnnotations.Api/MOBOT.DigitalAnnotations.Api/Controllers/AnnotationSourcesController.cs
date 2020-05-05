using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    /// <summary>
    /// API Controller to handle AnnotationSource resource requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationSourcesController : DigitalAnnotationsControllerBase<AnnotationSourcesController>
    {       
        private readonly IAnnotationSourceService _annotationSourceService;

        public AnnotationSourcesController(IAnnotationSourceService annotationSourceService, ILogger<AnnotationSourcesController> logger) : base(logger)
        {
            _annotationSourceService = annotationSourceService;
        }

        /// <summary>
        /// Gets an AnnotationSource object using the supplied source URL.
        /// </summary>
        /// <param name="sourceUrl">The URL to the Annotation Source resource.</param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(AnnotationSourceModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBySourceUrl([FromQuery]string sourceUrl)
        {

            _logger.LogDebug($"Get By Source called with source url: {sourceUrl}.");
            if (string.IsNullOrWhiteSpace(sourceUrl))
                return BadRequest("A valid source url is required.");
            
            AnnotationSourceModel model = null;
            try
            {
                model = await _annotationSourceService.GetBySourceUrl(sourceUrl);
                if (model == null)
                    return NotFound($"No annotation source with url {sourceUrl} was found.");

                _logger.LogDebug($"Returning annotation source for url: {sourceUrl}.");               

                return Ok(model);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred retrieving annotation source using url {sourceUrl}.");
            }
        }

        /// <summary>
        /// Creates an AnnotationSource resource in the underlying data store if it does not exist.
        /// </summary>
        /// <param name="model">The AnnotationSource object containing values to be persisted.</param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(AnnotationSourceModel), StatusCodes.Status200OK)]        
        public async Task<IActionResult> Create([FromBody] AnnotationSourceModel model)
        {
            _logger.LogDebug($"Create Annotation Source called.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var source = await _annotationSourceService.GetBySourceUrl(model.SourceUrl);
                if (source == null)
                {
                    _logger.LogDebug($"Source not found for source URL {model.SourceUrl}. Attempting to create source.");
                    source = await _annotationSourceService.Add(model);
                }               

                return Ok(source);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred while creating annotation source.");
            }            

        }

        /// <summary>
        /// Updates an Annotation Source resource within the underlying data store using the supplied values.
        /// </summary>
        /// <param name="model">The Annotation Source Model containing the values to be updated.</param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(AnnotationSourceModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] AnnotationSourceModel model)
        {
            _logger.LogDebug($"Update Annotation Source called for Id: {model.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {       
                var source = await _annotationSourceService.Update(model);
                return Ok(source);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred while updating annotation source for id {model.Id}.");
            }
        }

        /// <summary>
        /// Gets a list of Annotation Target Model resources from the underlying data store using the supplied Annotation Source id.
        /// </summary>
        /// <param name="id">The id of the Annotation Source resource.</param>
        /// <returns></returns>
        [HttpGet(), Route("{id}/Targets")]
        [ProducesResponseType(typeof(IEnumerable<AnnotationTargetModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTargets(long id)
        {
            _logger.LogDebug($"Get Annotation Source Targets called.");
            if (id < 1)
                return BadRequest("A valid id is required.");

            try
            {
                var targets = await _annotationSourceService.GetAnnotationTarget(id);
                return Ok(targets);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred getting annotation source targets for id {id}.");
            }

           
        }
      
        //private async Task<string> GetImageByUrlAsync(string sourceUrl)
        //{
        //    using (var client = new HttpClient())
        //    using (var result = await client.GetAsync(sourceUrl)) {
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var imageBuf = await result.Content.ReadAsByteArrayAsync();
        //            return Convert.ToBase64String(imageBuf);
        //        }                   
        //    }

        //    return null;
        //}
    }
}