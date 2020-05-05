using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("AnnotationType", Schema = "dbo")]
    public class AnnotationType
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AnnotationTypeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid Type Name is required.")]
        public string TypeName { get; set; }

        public virtual ICollection<Annotation> Annotations { get; set; }

        public AnnotationType()
        {
            Annotations = new HashSet<Annotation>();
        }
    }
}
