using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class WebManifest
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; }

        // http://DigitalAnnotations.mobot.org/api/WebManifests/{id}
        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        public string TypeString => Constants.ManifestType;

        public string Label { get; set; }
        public string Description { get; set; }
        public string ViewingDirection => Constants.ViewingDirections.RightToLeft;

        public IEnumerable<WebSequence> Sequences { get; set; }
    }
}
