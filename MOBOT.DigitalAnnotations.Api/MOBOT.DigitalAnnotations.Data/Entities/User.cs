using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("User", Schema="security")]
    public partial class User
    {
        public User()
        {
            Groups = new HashSet<UserGroup>();
            CreatedAnnotations = new HashSet<Annotation>();
            UpdatedAnnotations = new HashSet<Annotation>();
        }
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "An Email is required.")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Password is required.")]
        [StringLength(20)]
        public string Password { get; set; }

        public virtual ICollection<UserGroup> Groups { get; set; }
        public virtual ICollection<Annotation> CreatedAnnotations { get; set; }
        public virtual ICollection<Annotation> UpdatedAnnotations { get; set; }
    }
}
