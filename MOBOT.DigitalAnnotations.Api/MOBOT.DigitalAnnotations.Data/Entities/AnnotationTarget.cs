using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("AnnotationTarget", Schema = "dbo")]
    public class AnnotationTarget
    {
        public AnnotationTarget()
        {
            Annotations = new HashSet<Annotation>();
        }

        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AnnotationTargetId { get; set; }

        [Required(ErrorMessage = "Location X is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid location x must be between {1} and {2}.")]
        [Column(TypeName ="decimal(18,8)")]
        public decimal CoordinateX { get; set; }

        [Required(ErrorMessage = "Location Y is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid location y must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal CoordinateY { get; set; }

        [Required(ErrorMessage = "Width is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid width must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Width { get; set; }

        [Required(ErrorMessage = "Height is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid height must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Height { get; set; }

        public long AnnotationSourceId { get; set; }

        #region Navigation Properties

        public virtual ICollection<Annotation> Annotations { get; set; }

        [ForeignKey("AnnotationSourceId")]
        public virtual AnnotationSource Source { get; set; }

        #endregion
    }
}
