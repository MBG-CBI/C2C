using System;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class WebAnnotationStorage : IWebAnnotationStorage
    {
        public Task<Uri> CreateWebAnnotation(WebAnnotation model)
        {
            throw new NotImplementedException();
        }
    }
}
