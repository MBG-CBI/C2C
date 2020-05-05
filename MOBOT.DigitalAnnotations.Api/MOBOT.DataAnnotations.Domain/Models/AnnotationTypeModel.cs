using System.ComponentModel.DataAnnotations;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class AnnotationTypeModel
    { 
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A valid Type Name is required.")]
        public string Name { get; set; }
    }
}
