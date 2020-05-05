using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("Tag", Schema="dbo")]
    public class Tag
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TagId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid tag name is required.")]
        [StringLength(30)]
        public string TagName { get; set; }

        public virtual ICollection<AnnotationTag> Annotations { get; set;  }
    }
}
