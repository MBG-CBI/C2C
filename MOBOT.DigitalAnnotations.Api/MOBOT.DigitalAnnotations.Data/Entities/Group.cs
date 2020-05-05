using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("Group", Schema = "security")]
    public partial class Group
    {
        public Group()
        {
            Users = new HashSet<UserGroup>();
        }

        [Key()]
        public int GroupId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Group Name is required.")]
        public string GroupName { get; set; }

        public virtual ICollection<UserGroup> Users { get; set; }
        public virtual ICollection<Annotation> Annotations { get; set; }
    }
}
