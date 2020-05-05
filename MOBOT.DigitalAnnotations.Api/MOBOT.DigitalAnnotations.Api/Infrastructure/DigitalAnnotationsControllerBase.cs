using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MOBOT.DigitalAnnotations.Api.Infrastructure
{
    public abstract class DigitalAnnotationsControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;

        public DigitalAnnotationsControllerBase(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected IActionResult LogAndCreateErrorResponse(Exception ex, string message)
        {
            _logger.LogError(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }

        protected IActionResult LogAndCreateErrorResponse(string message)
        {
            _logger.LogError(message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}
