using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Data.Entities
{
    [Table("UserGroups", Schema = "security")]
    public partial class UserGroup
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }
        [Required()]
        public int UserId { get; set; }
        [Required()]
        public int GroupId { get; set; }

        #region NavigationProperties
 
        public virtual User User { get; set; }
   
        public virtual Group Group { get; set; }

        #endregion
    }
}
