using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("VocabularyList", Schema="dbo")]
    public class VocabularyList
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VocabularyListId { get; set; }
        [Required()]
        public int ListTypeId { get; set; }
        [Required()]
        public long TermId { get; set; }

        public virtual VocabularyListType ListType { get; set; }
        public virtual VocabularyTerm Term { get; set; }
    }
}
