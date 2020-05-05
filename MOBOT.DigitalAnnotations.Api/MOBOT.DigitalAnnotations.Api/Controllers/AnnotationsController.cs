using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Api.Infrastructure;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using Newtonsoft.Json;
using MOBOT.DigitalAnnotations.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MOBOT.DigitalAnnotations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationsController : DigitalAnnotationsControllerBase<AnnotationsController>
    {
        private readonly IAnnotationService _annotationService;
        private readonly IConfigurationRoot _config;
        public AnnotationsController(IAnnotationService annotationService, IConfigurationRoot config, ILogger<AnnotationsController> logger) : base(logger) {
            _annotationService = annotationService;
            _config = config;
        }
        
        /// <summary>
        /// Creates an Annotation resource in the underlying data store using the supplied values
        /// </summary>
        /// <param name="model">The Annotation Model containing data to persist.</param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(AnnotationModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAnnotation([FromBody]AnnotationModel model)
        {
            _logger.LogDebug("Post Annotation called.");

            try
            {
                if (ModelState.IsValid)
                {
                    model = await _annotationService.Add(model);
                    _logger.LogDebug($"Annotation {model.Id} created.");
                    return Created($"{_config["ApiBaseUrl"]}/Annotations/{model.Id}", model);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred creating annotation. Model: {JsonConvert.SerializeObject(model)}.");
            }
           
        }

        /// <summary>
        /// Updates an Annotation resource in the underlying data store using the supplied values.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(AnnotationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAnnotation([FromBody]AnnotationModel model) {

            _logger.LogDebug("Put Annotation called.");
            try
            {
                if (ModelState.IsValid)
                {
                    model = await _annotationService.Update(model);
                    _logger.LogDebug($"Annotation {model.Id} updated.");
                    return Ok(model);
                }
                return BadRequest(ModelState);
            }
            catch (NotFoundException nfEx) {
                return new NotFoundObjectResult(nfEx.Message);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred updating annotation. Model: {JsonConvert.SerializeObject(model)}.");
            }
        }

        [HttpPost(), Route("List")]
        [ProducesResponseType(typeof(IEnumerable<AnnotationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnnotationList([FromBody]AnnotationRequestModel request) {

            try
            {
                var list = await _annotationService.GetFilteredList(request.sourceId, request.targetId, request.SearchText, request.Filters);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred getting Annotation list.");
            }
        }
    }
}