using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    /// <summary>
    /// The target box of an annotation on the canvas.
    /// </summary>
    public class AnnotationTargetModel
    {
        /// <summary>
        /// The Id to the Annotation Target resource.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The Source id for the resource's Source.
        /// </summary>
        [Required(ErrorMessage = "A valid source id is required.")]
        public long SourceId { get; set; }

        /// <summary>
        /// The resource's Annotation Source.
        /// </summary>
        public AnnotationSourceModel Source { get; set; }

        /// <summary>
        /// The resource's X coordinate.
        /// </summary>
        [Required(ErrorMessage = "Location X is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid location x must be between {1} and {2}.")]
        public decimal CoordinateX { get; set; }
        /// <summary>
        /// The resource's Y coordinate.
        /// </summary>
        [Required(ErrorMessage = "Location Y is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid location y must be between {1} and {2}.")]
        public decimal CoordinateY { get; set; }
        /// <summary>
        /// The Width of the resource box.
        /// </summary>
        [Required(ErrorMessage = "Width is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid width must be between {1} and {2}.")]
        public decimal Width { get; set; }
        /// <summary>
        /// The Height of the resource box
        /// </summary>
        [Required(ErrorMessage = "Height is required")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid height must be between {1} and {2}.")]
        public decimal Height { get; set; }

    }
}
