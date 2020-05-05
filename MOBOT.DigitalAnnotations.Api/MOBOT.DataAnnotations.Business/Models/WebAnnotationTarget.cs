using System;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class WebAnnotationTarget
    {
        public Uri Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string TypeString { get; set;  }

        [JsonProperty(PropertyName = "format")]
        public string FormatString { get; set; }

        [JsonIgnore()]
        public string Source { get; set; }

        [JsonIgnore()]
        public decimal? CoordinateX { get; set; }

        [JsonIgnore()]
        public decimal? CoordinateY { get; set; }

        [JsonIgnore()]
        public decimal? Width { get; set; }

        [JsonIgnore()]
        public decimal? Height { get; set; }
    }
}
