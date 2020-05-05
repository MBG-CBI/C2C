using System;
using System.Collections.Generic;
using System.Text;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserGroupModel> Users { get; set; }
    }
}
