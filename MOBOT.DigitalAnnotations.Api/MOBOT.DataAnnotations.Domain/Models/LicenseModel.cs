namespace MOBOT.DigitalAnnotations.Domain.Models
{

    public class LicenseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string IconUrl { get; set; }
        public int Sequence { get; set; }
        public string LicenseUrl { get; set; }
    }
}
