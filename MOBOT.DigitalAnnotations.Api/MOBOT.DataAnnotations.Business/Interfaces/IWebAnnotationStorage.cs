using System;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IWebAnnotationStorage
    {
        /// <summary>
        /// Creates a new web resource using the supplied WebAnnotation values.
        /// </summary>
        /// <param name="model">The Web Annotation Model used when creating the resource.</param>
        /// <returns></returns>
        Task<Uri> CreateWebAnnotation(WebAnnotation model);
    }
}
