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
    public class VocabularyController : DigitalAnnotationsControllerBase<VocabularyController>
    {
        private readonly IVocabularySearchService _vocabularySearchService;
        public VocabularyController(IVocabularySearchService vocabularySearchService, ILogger<VocabularyController> logger): base(logger)
        {
            _vocabularySearchService = vocabularySearchService;
        }

        /// <summary>
        /// Performs lookup...
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet(), Route("Lookup")]
        [ProducesResponseType(typeof(IEnumerable<VocabularyLookupResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Lookup([FromQuery]string searchTerm) {
            try
            {
                var request = new VocabularyLookupRequestModel {
                    SearchTerm = searchTerm
                };
                var results = await _vocabularySearchService.Lookup(request);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred attempting to look up vocabulary. Search Term: {searchTerm}");
            }
        }
    }
}