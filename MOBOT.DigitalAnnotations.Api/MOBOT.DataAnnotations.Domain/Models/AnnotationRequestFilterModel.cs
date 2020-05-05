using MOBOT.DigitalAnnotations.Domain.Enumerations;
using System;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class AnnotationRequestFilterModel
    {
        public AnnotationFilterTypes FilterType { get; set; }
        public int? FilterId { get; set; }
        public DateTime? Date { get; set; }
    }
}
