using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class WebCanvas
    {
        // http://DigitalAnnotations.mobot.org/api/WebCanvases/{id}
        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "@type")]
        public string TypeString => Constants.CanvasType;

        public string Label { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public IEnumerable<WebAnnotation> Images { get; set; }
    }
}
