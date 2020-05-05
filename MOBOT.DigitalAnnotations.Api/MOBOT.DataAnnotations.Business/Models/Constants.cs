namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class Constants
    {
        public const string ManifestType = "sc:Manifest";
        public const string AnnotationType = "oa:Annotation";
        public const string ImageType = "Image";
        public const string SequenceType = "sc:Sequence";
        public const string CanvasType = "sc:Canvas";
        
        public class ViewingDirections {
            public const string RightToLeft = "right-to-left";
        }

        public class MediaTypes {
            public const string JPegImage = "image/jpeg";
            public const string ApplicationJSon = "application/json";
            public const string ApplicationJSonLD = "application/ld+json";
        }
    }
}
