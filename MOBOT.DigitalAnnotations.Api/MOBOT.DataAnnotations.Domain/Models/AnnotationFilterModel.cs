namespace MOBOT.DigitalAnnotations.Domain.Models
{
    public class AnnotationFilterModel
    {        
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string DisplayName => $"{Name} [{Count}]";
    }
}
