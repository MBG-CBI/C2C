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
    public class WebManifestService : IWebManifestService
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<WebManifestService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AnnotationSource> _annotationSources;
        private readonly DbSet<Annotation> _annotations;

        private static string _apiBaseUrl;

        public WebManifestService(IUnitOfWork unitOfWork, IConfigurationRoot config, ILogger<WebManifestService> logger)
        {
            _config = config;
            _apiBaseUrl = _config["ApiBaseUrl"];
            _logger = logger;
            _logger.LogDebug($"Api base is {_apiBaseUrl}");
            _unitOfWork = unitOfWork;
            _annotationSources = _unitOfWork.Set<AnnotationSource>();
            _annotations = _unitOfWork.Set<Annotation>();
        }

        public async Task<WebManifest> GetByAnnotationSourceId(long id)
        {
            var annotationSource = await _annotationSources.SingleOrDefaultAsync(s => s.AnnotationSourceId == id);

            var annotations = _annotations
                .Include(a => a.Target)
                .ThenInclude(t => t.Source)
                .Include(a => a.License)
                .Include(a => a.AnnotationType)
                .Include(a => a.Annotations)
                .Include(a => a.Tags).ThenInclude(t => t.Tag)
                .Where(a => a.Target.AnnotationSourceId == id && a.ParentId == null);
            var images = new List<WebAnnotation>();            
            await annotations.ForEachAsync(a =>
            {
                images.AddRange(WebAnnotationService.ConvertAnnotationsToWebAnnotation(a, _apiBaseUrl));
            });

            var canvases = new List<WebCanvas>();
            canvases.Add(new WebCanvas { Id = $"{_apiBaseUrl}/api/WebCanvases/{annotationSource.AnnotationSourceId}", Images = images });

            var sequence = new WebSequence();
            sequence.Canvases = canvases;

            var sequences = new List<WebSequence>();
            sequences.Add(sequence);

            var manifest = new WebManifest
            {
                Id = annotationSource.RerumStorageUrl,
                Context = $"{_apiBaseUrl}/api/WebManifests/{annotationSource.AnnotationSourceId}/context.json",
                Sequences = sequences
            };

            return manifest;
        }

        
    }
}
