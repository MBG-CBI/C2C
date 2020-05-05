using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("Annotation", Schema = "dbo")]
    public class Annotation
    {
        public Annotation()
        {
            Annotations = new HashSet<Annotation>();
            Tags = new HashSet<AnnotationTag>();
        }
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AnnotationId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Annotation Body is required.")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Created User is required.")]
        public int CreatedUserId { get; set; }

        public int? UpdatedUserId { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public long AnnotationTargetId { get; set; }

        [Required(ErrorMessage = "A user group is required.")]
        public int GroupId { get; set; }

        public int? LicenseId { get; set; }

        public int? AnnotationTypeId { get; set; }
        public long? ParentId { get; set; }

        #region Navigation Properties

        [ForeignKey("AnnotationTargetId")]
        public virtual AnnotationTarget Target { get; set; }

        public virtual User UpdatedUser { get; set; }
        public virtual User CreatedUser { get; set; }

        public virtual Group Group { get; set; }
        public virtual License License { get; set; }
        public virtual AnnotationType AnnotationType { get; set; }
        public virtual Annotation Parent { get; set; }
        public virtual ICollection<Annotation> Annotations { get; set; }
        public virtual ICollection<AnnotationTag> Tags { get; set; }
        #endregion

    }
}
