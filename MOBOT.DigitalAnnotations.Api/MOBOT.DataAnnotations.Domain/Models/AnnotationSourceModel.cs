using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    /// <summary>
    /// The Source of an Annotation.
    /// </summary>
    public class AnnotationSourceModel
    {
        /// <summary>
        /// The id to the AnnotationSource resource.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The url to the source image \ file.
        /// </summary>
        [Required(ErrorMessage = "A source url is required.", AllowEmptyStrings = false)]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A valid source url must be at least {2} in length.")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// The url to rerum storage.
        /// </summary>
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A valid rerum storage url must be at least {2} in length.")]
        public string RerumStorageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid image height must be between {1} and {2}.")]
        public decimal? ImageHeight { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid image width must be between {1} and {2}.")]
        public decimal? ImageWidth { get; set; }

        /// <summary>
        /// The user name who created the resource.
        /// </summary>
        public string CreatedUser { get; set; }

        /// <summary>
        /// The user name who last updated the resource.
        /// </summary>
        public string UpdatedUser { get; set; }

        /// <summary>
        /// The creation date of the resource.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The last update date of the resource.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// A list of Annotations that are tied to this resource.
        /// </summary>
        public IEnumerable<AnnotationTargetModel> Targets { get; set; }

        public AnnotationSourceModel()
        {
            Targets = new List<AnnotationTargetModel>();
        }
    }
}
