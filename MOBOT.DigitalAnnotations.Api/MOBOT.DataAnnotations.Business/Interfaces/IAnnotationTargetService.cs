using System.Collections.Generic;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IAnnotationTargetService
    {
        /// <summary>
        /// Gets a list of annotation resources for the supplied target id
        /// </summary>
        /// <param name="id">The id of the target resource.</param>
        /// <returns></returns>
        Task<IEnumerable<AnnotationModel>> GetAnnotations(long id); 
    }
}
