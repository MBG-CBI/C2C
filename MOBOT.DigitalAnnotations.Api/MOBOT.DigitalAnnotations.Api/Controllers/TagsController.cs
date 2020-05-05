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
    /// Controller to handle requests for Tag resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : DigitalAnnotationsControllerBase<TagsController>
    {
        private readonly ITagService _service;
        public TagsController(ITagService tagService, ILogger<TagsController> logger): base(logger)
        {
            _service = tagService;
        }

        [HttpGet(), Route("List")]
        [ProducesResponseType(typeof(IEnumerable<TagModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList() {
            try
            {
                _logger.LogDebug($"Get Tag List Called.");
                var list = await _service.ListAsync();
                return Ok(list);

            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred retreiving Tag List.");
            }
        }

    }
}