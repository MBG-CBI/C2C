using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IWebAnnotationService
    {
        /// <summary>
        /// Gets the web annotation for the supplied annotation id.
        /// </summary>
        /// <param name="id">The id of the annotation resource.</param>
        /// <returns></returns>
        Task<WebAnnotation> GetAnnotationById(int id);
    }
}
