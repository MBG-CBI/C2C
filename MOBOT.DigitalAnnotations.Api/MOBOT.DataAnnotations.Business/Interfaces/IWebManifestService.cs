using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IWebManifestService
    {
        Task<WebManifest> GetByAnnotationSourceId(long id);
    }
}
