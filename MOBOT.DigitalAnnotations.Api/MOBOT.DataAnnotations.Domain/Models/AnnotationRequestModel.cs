using System.Collections.Generic;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class AnnotationRequestModel
    {
        public long? sourceId { get; set; }
        public long? targetId { get; set; }
        public string SearchText { get; set; }
        public IEnumerable<AnnotationRequestFilterModel> Filters { get; set; }
    }
}
