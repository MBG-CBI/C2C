using System;
using System.Collections.Generic;
using System.Text;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class UserGroupModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set;}
    }
}
