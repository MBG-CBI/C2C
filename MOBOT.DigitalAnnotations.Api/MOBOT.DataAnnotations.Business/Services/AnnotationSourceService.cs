using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AnnotationSourceService : IAnnotationSourceService
    {
        private readonly ILogger<AnnotationSourceService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AnnotationSource> _annotationSources;
        private readonly DbSet<AnnotationTarget> _annotationTargets;

        public AnnotationSourceService(IUnitOfWork unitOfWork, ILogger<AnnotationSourceService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _annotationSources = _unitOfWork.Set<AnnotationSource>();
            _annotationTargets = _unitOfWork.Set<AnnotationTarget>();
        }

        public async Task<AnnotationSourceModel> Add(AnnotationSourceModel model)
        {            
            var entity = model.ToEntity();
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedUser = "Anonymous";
            entity.UpdatedUser = null;
            entity.UpdatedDate = null;
            await _annotationSources.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();            
            return entity.ToModel();
        }

        public async Task<AnnotationSourceModel> Update(AnnotationSourceModel model)
        {
            var entity = await _annotationSources.SingleOrDefaultAsync(a => a.AnnotationSourceId == model.Id);            
            model.PopulateEntity(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.ToModel();
        }

        public async Task<AnnotationSourceModel> Add(string sourceUrl) {
            var model = new AnnotationSourceModel { SourceUrl = sourceUrl };
            return await Add(model);
        }
        
        public Task<AnnotationSourceModel> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<AnnotationSourceModel> GetBySourceUrl(string url)
        {
            var entity = await _annotationSources.Include(a=>a.Targets).SingleOrDefaultAsync(a => a.SourceUrl == url);
            return entity.ToModel();
        }

        public async Task<IEnumerable<AnnotationTargetModel>> GetAnnotationTarget(long id)
        {
            var targets = await _annotationTargets.Where(t => t.AnnotationSourceId == id).ToListAsync();
            var rt = targets?.ConvertAll(t => t.ToModel());
            return rt;
        }

    }
}
