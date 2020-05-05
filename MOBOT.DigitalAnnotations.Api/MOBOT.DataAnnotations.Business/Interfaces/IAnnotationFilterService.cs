using MOBOT.DigitalAnnotations.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IAnnotationFilterService
    {
        Task<IEnumerable<AnnotationFilterTypeModel>> GetFiltersForSource(int sourceId);
    }
}
