using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("AnnotationSource", Schema = "dbo")]
    public class AnnotationSource
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AnnotationSourceId { get; set; }

        [Required(ErrorMessage = "A source url is required.", AllowEmptyStrings = false)]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A valid source url must be at least {2} in length.")]
        public string SourceUrl { get; set; }

        [StringLength(500, MinimumLength = 1, ErrorMessage = "A valid rerum storage url must be at least {2} in length.")]
        public string RerumStorageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid image height must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal? ImageHeight { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid image width must be between {1} and {2}.")]
        [Column(TypeName = "decimal(18,8)")]
        public decimal? ImageWidth { get; set; }

        [Required(ErrorMessage = "Created User is required.")]
        public string CreatedUser { get; set; }

        public string UpdatedUser { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        #region Navigation Properties
        public virtual ICollection<AnnotationTarget> Targets { get; set; }
        #endregion
    }
}
