using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class WebAnnotation
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context => "http://www.w3.org/ns/anno.jsonld";

        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        public string TypeString => Constants.AnnotationType;

        // http://DigitalAnnotations/Canvases/{id}
        public string On { get; set; }

        [JsonProperty(PropertyName = "target")]
        public WebAnnotationTarget Target { get; set; }

        [JsonProperty(PropertyName = "body")]
        public dynamic Body { get; set; }

        [JsonProperty(PropertyName = "rights")]
        public string LicenseUrl { get; set; }
        [JsonProperty(PropertyName = "motivation")]
        public string Motivation { get; set; }

        [JsonIgnore]
        public long CanvasId { get; set; }
    }
}
