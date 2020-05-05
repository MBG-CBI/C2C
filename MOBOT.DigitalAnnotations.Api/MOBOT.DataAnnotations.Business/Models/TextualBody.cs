using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class TextualBody
    {

        [JsonProperty(PropertyName = "type")]
        public string TypeString => "TextualBody";

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }

        [JsonProperty(PropertyName = "format")]
        public string Format => "text/plain";
    }
}
