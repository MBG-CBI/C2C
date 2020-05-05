using System.Collections.Generic;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IAnnotationSourceService
    {
        /// <summary>
        /// Gets the Annotation Source resource from the underlying data store 
        ///     using the supplied id.
        /// </summary>
        /// <param name="id">The id of the resource to retrieve.</param>
        /// <returns></returns>
        Task<AnnotationSourceModel> GetById(long id);

        /// <summary>
        /// Gets the Annotation Source resource from the underlying data store 
        ///     using the supplied source url.
        /// </summary>
        /// <param name="url">The url to the original source image \ file.</param>
        /// <returns></returns>
        Task<AnnotationSourceModel> GetBySourceUrl(string url);

        /// <summary>
        /// Adds the Annotation Source resource to the underlying data store using the supplied values.
        /// </summary>
        /// <param name="model">The annotation model object containing values to be added.</param>
        /// <returns></returns>
        Task<AnnotationSourceModel> Add(AnnotationSourceModel model);

        /// <summary>
        /// Updates the Annotation Source resource in the underlying data store using the supplied values.
        /// </summary>
        /// <param name="model">The annotation model object containing values to be updated.</param>
        /// <returns></returns>
        Task<AnnotationSourceModel> Update(AnnotationSourceModel model);

        /// <summary>
        /// Gets a list of targets for the specified Annotation Source id.
        /// </summary>
        /// <param name="id">The id of the Annotation Source resource.</param>
        /// <returns>A list of targets for the supplied Annotation Source id if found, otherwise an empty list.</returns>
        Task<IEnumerable<AnnotationTargetModel>> GetAnnotationTarget(long id);

    }
}
