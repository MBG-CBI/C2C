using MOBOT.DigitalAnnotations.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class AnnotationFilterTypeModel
    {
        public AnnotationFilterTypes Type { get; set; }
        public string FilterName => Type.ToString();

        public IEnumerable<AnnotationFilterModel> Filters { get; set; }
    }
}
