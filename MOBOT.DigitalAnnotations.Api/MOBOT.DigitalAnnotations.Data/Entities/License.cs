using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("License", Schema = "dbo")]
    public class License
    {

        public int LicenseId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A license code is required.")]
        public string Code { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A license code is required.")]
        [StringLength(50)]
        public string DisplayName { get; set; }
        [StringLength(256)]
        public string IconUrl { get; set; }
        public int Sequence { get; set; }
        [StringLength(256)]
        public string LicenseUrl { get; set; }

        public virtual ICollection<Annotation> Annotations {get; set;}

        public License()
        {
            Annotations = new HashSet<Annotation>();
        }
    }
}
