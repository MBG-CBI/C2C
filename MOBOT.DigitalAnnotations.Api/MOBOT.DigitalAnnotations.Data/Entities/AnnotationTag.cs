using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("AnnotationTags", Schema = "dbo")]
    public class AnnotationTag
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnnotationTagId { get; set; }
        [Required(ErrorMessage = "An annotation id is required.")]
        public long AnnotationId { get; set; }
        [Required(ErrorMessage = "A tag id is required.")]
        public int TagId { get; set; }

        public virtual Annotation Annotation { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
