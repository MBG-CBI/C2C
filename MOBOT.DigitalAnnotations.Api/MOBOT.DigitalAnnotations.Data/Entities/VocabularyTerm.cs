using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("VocabularyTerm", Schema="dbo")]
    public class VocabularyTerm
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VocabularyTermId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Term { get; set; }

        public virtual ICollection<VocabularyList> Lists { get; set; }
    }
}
