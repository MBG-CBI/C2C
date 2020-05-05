using System.Collections.Generic;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<UserGroupModel> Groups { get; set; }
    }
}
