using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("VocabularyListType", Schema ="dbo")]
    public class VocabularyListType
    {

        public VocabularyListType()
        {
            Terms = new HashSet<VocabularyList>();
        }

        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VocabularyListTypeId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(10)]
        public string TypeTerm { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string TypeDescription { get; set; }

        public virtual ICollection<VocabularyList> Terms { get; set; }
    }
}
