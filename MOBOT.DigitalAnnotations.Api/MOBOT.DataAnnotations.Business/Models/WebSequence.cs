using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class WebSequence
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; }

        // http://DigitalAnnotations.mobot.org/api/WebManifest/{id}/Sequence
        [JsonProperty(PropertyName = "@id")]
        public Uri Id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        public string TypeString => Constants.SequenceType;

        public IEnumerable<WebCanvas> Canvases { get; set;  }
    }
}
