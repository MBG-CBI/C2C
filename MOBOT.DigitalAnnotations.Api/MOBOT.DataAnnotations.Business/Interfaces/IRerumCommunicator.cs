using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IRerumCommunicator
    {
        /// <summary>
        /// Posts a manifest to the rerum server and returns a RerumPostManifestResponse object.
        /// </summary>
        /// <param name="manifest">A IIIF complaint manifest.</param>
        /// <returns></returns>
        Task<RerumManifestResponse> PostManifest(WebManifest manifest);

        /// <summary>
        /// Updates a manifest in the rerum server and returns a RerumPostManifestResponse object.
        /// </summary>
        /// <param name="manifest">A IIIF complaint manifest.</param>
        /// <returns></returns>
        Task<RerumManifestResponse> PutManifest(WebManifest manifest);
    }
}
