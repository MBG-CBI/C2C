using System.Collections.Generic;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IAnnotationService
    {
        /// <summary>
        /// Gets the Annotation resource from the underlying data store using the supplied id.
        /// </summary>
        /// <param name="id">The id of the resource to retrieve.</param>
        /// <returns></returns>
        Task<AnnotationModel> GetById(long id);
        
        /// <summary>
        /// Adds an Annotation to the underlying data store using the supplied values. 
        /// </summary>
        /// <param name="model">The Annotation Model containing values to be added.</param>
        /// <returns></returns>
        Task<AnnotationModel> Add(AnnotationModel model);

        /// <summary>
        /// Updates an Annotation in the underlying data store using the supplied values.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AnnotationModel> Update(AnnotationModel model);

        Task<IEnumerable<AnnotationModel>> GetFilteredList(long? sourceId, long? targetId, string searchText, IEnumerable<AnnotationRequestFilterModel> filters);

    }
}
