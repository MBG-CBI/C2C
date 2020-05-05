using System.ComponentModel.DataAnnotations;

namespace MOBOT.DigitalAnnotations.Api.Models
{
    public class LoginRequestModel
    {
        [Required( AllowEmptyStrings = false, ErrorMessage = "A valid user name is required.")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid password is required.")]
        public string Password { get; set; }
    }
}
