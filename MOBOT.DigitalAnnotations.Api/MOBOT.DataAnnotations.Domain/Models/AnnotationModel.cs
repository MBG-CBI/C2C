using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOBOT.DigitalAnnotations.Domain.Models
{
    /// <summary>
    /// Model of a digital annotation.
    /// </summary>
    public class AnnotationModel
    {
        /// <summary>
        /// Gets\Sets id to the Annotation resource.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets the subject of the annotation.
        /// </summary>
        public string Subject => Body != null ? Body.Length <= 30 ? Body : $"{Body.Substring(0, 27)}..." : "";

        /// <summary>
        /// The body or "text" for the resource.
        /// </summary>
        [Display(Name = "Annotation Body")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Annotation Body is required.")]        
        public string Body { get; set; }

        [Required(ErrorMessage = "A valid user is required.")]
        public int CreatedUserId { get; set; }

        /// <summary>
        /// Gets\Sets the user name who created the resource.
        /// </summary>
        public string CreatedUser { get; set; }

        public int? UpdatedUserId { get; set; }
        /// <summary>
        /// Gets\Sets the user name who last updated the resource.
        /// </summary>
        public string UpdatedUser { get; set; }
        /// <summary>
        /// Gets\Sets the creation date of the resource.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets\Sets the last update date of the resource.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets\Sets the Target id for the resource's Target.
        /// </summary>
        [Required(ErrorMessage = "A target is required.")]
        public long TargetId { get; set; }

        [Required(ErrorMessage = "A valid group is required.")]
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public int? LicenseId { get; set; }
        public int? AnnotationTypeId { get; set; }
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets\Sets the resource's Target resource.
        /// </summary>
        public AnnotationTargetModel Target { get; set; }
        
        public LicenseModel License { get; set; }
        public AnnotationTypeModel AnnotationType { get; set; }
        public IEnumerable<AnnotationModel> Annotations { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }

        public AnnotationModel()
        {
            Target = new AnnotationTargetModel();            
        }
    }
}
