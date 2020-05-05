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
    public class LicenseController : DigitalAnnotationsControllerBase<LicenseController>
    {
        private readonly ILicenseService _service;
        public LicenseController(ILicenseService licenseService, ILogger<LicenseController> logger): base(logger)
        {
            _service = licenseService;
        }

        [HttpGet(), Route("List")]
        [ProducesResponseType(typeof(IEnumerable<LicenseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList() {
            try
            {
                _logger.LogDebug($"Get License List Called.");
                var list = await _service.GetListAsync();                    
                return Ok(list);
                
            }
            catch (Exception ex)
            {
                return LogAndCreateErrorResponse(ex, $"An error occurred retreiving License List.");
            }
        }
    }
}