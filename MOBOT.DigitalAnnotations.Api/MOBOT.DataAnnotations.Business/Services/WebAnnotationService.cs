using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class WebAnnotationService : IWebAnnotationService
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<WebAnnotationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Annotation> _annotations;

        private static string _apiBaseUrl;

        public WebAnnotationService(IUnitOfWork unitOfWork, IConfigurationRoot config, ILogger<WebAnnotationService> logger)
        {
            _config = config;
            _apiBaseUrl = _config["ApiBaseUrl"];
            _logger = logger;
            _unitOfWork = unitOfWork;
            _annotations = _unitOfWork.Set<Annotation>();
        }

        public async Task<WebAnnotation> GetAnnotationById(int id)
        {
            var annotation = await _annotations
                .Include(a=> a.Target)
                .ThenInclude(t=>t.Source)
                .Include(a => a.License)
                .Include(a => a.AnnotationType)
                .Include(a => a.Annotations)
                .Include(a => a.Tags).ThenInclude(t => t.Tag)
                .SingleOrDefaultAsync(a => a.AnnotationId == id);
            return ConvertAnnotationToWebAnnotation(annotation, _apiBaseUrl);
        }

        public static WebAnnotation ConvertAnnotationToWebAnnotation(Annotation annotation, string baseUrl)
        {
            WebAnnotation rt = null;

            if (annotation != null) {

                var source = !annotation.ParentId.HasValue ? annotation.Target.Source.SourceUrl : null;
                var coordinateX = !annotation.ParentId.HasValue ? annotation.Target.CoordinateX : (decimal?)null;
                var coordinateY = !annotation.ParentId.HasValue ? annotation.Target.CoordinateY : (decimal?)null;
                var height = !annotation.ParentId.HasValue ? annotation.Target.Height : (decimal?)null;
                var width = !annotation.ParentId.HasValue ? annotation.Target.Width : (decimal?)null;
                var id = !annotation.ParentId.HasValue ? new Uri($"{source}#xywh={coordinateX},{coordinateY},{width},{height}") :
                    new Uri($"{baseUrl}/api/WebAnnotations/{annotation.ParentId}");
                var type = !annotation.ParentId.HasValue ? Constants.ImageType : Constants.AnnotationType;
                var format = !annotation.ParentId.HasValue ? Constants.MediaTypes.JPegImage : null;

                rt = new WebAnnotation
                {
                    Id = $"{_apiBaseUrl}/api/WebAnnotations/{annotation.AnnotationId}",
                    Body = CreateAnnotationBody(annotation),

                    Target = new WebAnnotationTarget
                    {
                        Id = id,
                        TypeString = type,
                        FormatString = format,
                        Source = source,
                        CoordinateX = coordinateX,
                        CoordinateY = coordinateY,
                        Height = height,
                        Width = width
                    },
                    LicenseUrl = annotation.License?.LicenseUrl,
                    CanvasId = annotation.Target.AnnotationSourceId,
                    On = !annotation.ParentId.HasValue ? $"{baseUrl}/api/Canvases/{annotation.Target.Source.AnnotationSourceId}" : null,
                    Motivation = ConvertTypeToMotivation(annotation.AnnotationType)
            
                };
            }

            return rt;
        }

        private static dynamic CreateAnnotationBody(Annotation annotation)
        {
            var tags = annotation.Tags?.ToList();
            if (tags != null && tags.Count > 0)
            {
                var body = new List<TextualBody>();
                body.Add(new TextualBody { Value = annotation.Body });
                body.AddRange(tags.ConvertAll(t => new TextualBody
                {
                    Value = t.Tag.TagName,
                    Purpose = "tagging"
                }));
                return body;
            }
            else {
                return new TextualBody { Value = annotation.Body };
            }
        }

        public static IEnumerable<WebAnnotation> ConvertAnnotationsToWebAnnotation(Annotation annotation, string baseUrl) {
            var list = new List<WebAnnotation>();
            if (annotation != null) {
                var wa = ConvertAnnotationToWebAnnotation(annotation, baseUrl);
                list.Add(wa);
                annotation.Annotations?.ToList().ForEach(a => {
                    list.AddRange(ConvertAnnotationsToWebAnnotation(a, baseUrl));
                });
            }
            return list;
        
        }

        private static string ConvertTypeToMotivation(AnnotationType annotationType)
        {
            if (annotationType == null)
                return null;

            var at = annotationType.TypeName.ToLower().Trim();
            switch (at) {
                case "bookmark":
                    return "bookmarking";
                case "classify":
                    return "classifying";
                case "comment":
                    return "commenting";
                case "describe":
                    return "describing";
                case "edit":
                    return "editing";
                case "highlight":
                    return "highlighting";
                case "identify":
                    return "identifying";
                case "link":
                    return "linking";
                case "moderate":
                    return "moderating";
                case "question":
                    return "questioning";
                case "reply":
                    return "replying";
                case "tag":
                    return "tagging";
                default:
                    return null;
            }
        }
    }
}
