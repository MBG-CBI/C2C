using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("VocabularyListView", Schema="dbo")]
    public class VocabularyListView
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long VocabularyListId { get; set; }
        public int ListTypeId { get; set; }
        public long TermId { get; set; }
        public string TypeTerm { get; set; }
        public string TypeDescription { get; set; }
        public string Term { get; set; }
        public string SearchTerm { get; set; }
    }
}
