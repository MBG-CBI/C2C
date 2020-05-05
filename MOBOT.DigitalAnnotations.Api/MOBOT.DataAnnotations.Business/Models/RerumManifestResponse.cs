using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class RerumManifestResponse
    {
        public int Code { get; set; }
       
        [JsonProperty(PropertyName = "new_obj_state")]
        [DataMember(Name = "new_obj_state")]
        public ObjectState NewObjectState { get; set; }

        [JsonProperty(PropertyName = "iiif_validation")]
        [DataMember(Name = "iiif_validation")]
        public IIIFValidation Validation { get; set; }
    }

    public class IIIFValidation {
        public IEnumerable<string> Warnings { get; set; }
        public string Error { get; set; }
        public bool Okay { get; set; }
    }

    public class RerumResponse {

        public int Code { get; set; }

        [JsonProperty(PropertyName = "@id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "new_obj_state")]
        public ObjectState NewObjectState { get; set; }
    }

    public class ObjectState
    {
        [JsonProperty(PropertyName = "@id")]
        [DataMember(Name = "@id")]
        public string Id { get; set; }
    }
}
