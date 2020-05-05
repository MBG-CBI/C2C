using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Business.Models;
using MOBOT.DigitalAnnotations.Domain.Exceptions;
using System.Linq;
using System.Collections.Generic;
using MOBOT.DigitalAnnotations.Domain.Enumerations;
using System.Linq.Expressions;
using MOBOT.DigitalAnnotations.Business.Utils;
using MOBOT.DigitalAnnotations.Business.Extensions;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AnnotationService : IAnnotationService
    {
        private readonly ILogger<AnnotationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Annotation> _annotations;
        private readonly DbSet<AnnotationSource> _annotationSources;
        private readonly DbSet<AnnotationTarget> _annotationTargets;
        private readonly DbSet<AnnotationTag> _annotationTags;
        private readonly IRerumCommunicator _rerum;
        private readonly IWebManifestService _manifests;

        public AnnotationService(IUnitOfWork unitOfWork, IRerumCommunicator rerum,
            IWebManifestService manifestService, ILogger<AnnotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _rerum = rerum;
            _manifests = manifestService;
            _annotations = _unitOfWork.Set<Annotation>();
            _annotationSources = _unitOfWork.Set<AnnotationSource>();
            _annotationTargets = _unitOfWork.Set<AnnotationTarget>();
            _annotationTags = _unitOfWork.Set<AnnotationTag>();
        }
        
        public Task<AnnotationModel> GetById(long id)
        {
            throw new NotImplementedException();
        }

       
        public async Task<AnnotationModel> Add(AnnotationModel model)
        {
            var created = await AddAnnotation(model);
            var manifest = await _manifests.GetByAnnotationSourceId(created.Target.SourceId);
            var source = await _annotationSources.SingleOrDefaultAsync(s => s.AnnotationSourceId == created.Target.SourceId);
            RerumManifestResponse rerumResponse = null;
            // should we care if we save to rerum at this point?
            try
            {
                if (string.IsNullOrWhiteSpace(source.RerumStorageUrl))
                {
                    // Post the manifest
                    rerumResponse = await _rerum.PostManifest(manifest);
                }
                else
                {
                    rerumResponse = await _rerum.PutManifest(manifest);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred saving manifest to rerum.");
            }
            
            if (rerumResponse != null && (rerumResponse.Code == 200 || rerumResponse.Code == 201))
            {
                if (source.RerumStorageUrl != rerumResponse.NewObjectState.Id)
                {
                    source.RerumStorageUrl = rerumResponse.NewObjectState.Id;
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return created;
        }

        public async Task<AnnotationModel> Update(AnnotationModel model)
        {
            var updated = await UpdateAnnotation(model);
            var manifest = await _manifests.GetByAnnotationSourceId(updated.Target.SourceId);
            var source = await _annotationSources.SingleOrDefaultAsync(s => s.AnnotationSourceId == updated.Target.SourceId);
            RerumManifestResponse rerumResponse = null;
            // should we care if we save to rerum at this point?
            try
            {
                if (string.IsNullOrWhiteSpace(source.RerumStorageUrl))
                {
                    // Post the manifest
                    rerumResponse = await _rerum.PostManifest(manifest);
                }
                else
                {
                    rerumResponse = await _rerum.PutManifest(manifest);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred saving manifest to rerum.");
            }

            if (rerumResponse != null && (rerumResponse.Code == 200 || rerumResponse.Code == 201))
            {
                if (source.RerumStorageUrl != rerumResponse.NewObjectState.Id)
                {
                    source.RerumStorageUrl = rerumResponse.NewObjectState.Id;
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return updated;
        }

        public async Task<IEnumerable<AnnotationModel>> GetFilteredList(long? sourceId, long? targetId, string searchText, IEnumerable<AnnotationRequestFilterModel> filters = null)
        {
            var isFiltered = filters != null && filters.Count() > 0;
            List<AnnotationModel> list = null;
            IQueryable<Annotation> query = null;
            if (targetId.HasValue && targetId.Value > 0)
            {
                query = _annotations.Where(a => a.AnnotationTargetId == targetId && (isFiltered || a.ParentId == null))
                    .Include(a => a.Target)
                    .Include(a => a.License)
                    .Include(a => a.Group)
                    .Include(a => a.CreatedUser)
                    .Include(a => a.UpdatedUser)
                    .Include(a => a.AnnotationType)
                    //.Include(a => a.Annotations.Where()
                    .Include(a => a.Tags).ThenInclude(at => at.Tag);
            }
            else {
                query = _annotations.Where(a => a.Target.AnnotationSourceId == sourceId && (isFiltered || a.ParentId == null))
                    .Include(a => a.Target)
                    .Include(a => a.License)
                    .Include(a => a.Group)
                    .Include(a => a.CreatedUser)
                    .Include(a => a.UpdatedUser)
                    .Include(a => a.AnnotationType)
                    //.Include(a => a.Annotations)
                    .Include(a => a.Tags).ThenInclude(at => at.Tag);
            }

            if (!string.IsNullOrWhiteSpace(searchText)) {
                query = query.Where(a => EF.Functions.Like(a.Body, $"%{searchText}%"));
            }


            if (filters != null) {

                var personFilters = filters.Where(f => f.FilterType == AnnotationFilterTypes.Person).ToList();
                if (personFilters.Count > 0) {
                    var exp = GetPersonFilterExpression(personFilters.Select(p => p.FilterId.Value));
                    query = query.Where(exp);                    
                }

                var typeFilters = filters.Where(f => f.FilterType == AnnotationFilterTypes.Type).ToList();
                if (typeFilters.Count > 0) {
                    var exp = GetTypeFilterExpression(typeFilters.Select(f => f.FilterId));
                    query = query.Where(exp);
                }

                var dateFilters = filters.Where(f => f.FilterType == AnnotationFilterTypes.Date).ToList();
                if (dateFilters.Count > 0) {
                    var exp = GetDateFilterExpression(dateFilters.Select(d => d.Date.Value));
                    query = query.Where(exp);
                }

                var tagFilters = filters.Where(f => f.FilterType == AnnotationFilterTypes.Tags).ToList();
                if (tagFilters.Count > 0) {
                    var exp = GetTagFilterExpression(tagFilters.Select(t => t.FilterId.Value));
                    query = query.Where(exp);
                }
            }

            if (query != null)
            {
                var annotations = await query.ToListAsync();
                list = annotations.ConvertAll(a => a.ToModel(!isFiltered));
            }
            return list;
        }

        private async Task<AnnotationModel> UpdateAnnotation(AnnotationModel model) {
            var entity = await _annotations
                .Include(a => a.CreatedUser)
                .Include(a => a.Target)
                .Include(a => a.Group)
                .Include(a => a.License)
                .Include(a => a.AnnotationType)
                .Include(a => a.Tags).ThenInclude(at => at.Tag)
                .FirstOrDefaultAsync(a => a.AnnotationId == model.Id);
            if (entity != null)
            {
                entity = model.ToEntity(entity);
                entity.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();
                var updated = entity.ToModel();
                return updated;
            }
            else {
                throw new NotFoundException($"Annotation with id {model.Id} not found.");
            }
        }

        private async Task<AnnotationModel> AddAnnotation(AnnotationModel model) {
            var target = model.Target?.ToEntity();
            if (target != null && target.AnnotationTargetId < 1)
                await _annotationTargets.AddAsync(target);

            var entity = model.ToEntity();
            entity.AnnotationTargetId = target.AnnotationTargetId;
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedUserId = model.CreatedUserId;
            entity.GroupId = model.GroupId;

            await _annotations.AddAsync(entity);
            
            await _unitOfWork.SaveChangesAsync();

            var added = await _annotations
                .Include(a => a.CreatedUser)
                .Include(a => a.Target)
                .Include(a => a.Group)
                .Include(a => a.License)
                .Include(a => a.AnnotationType)
                .Include(a => a.Tags).ThenInclude(at => at.Tag)
                .SingleOrDefaultAsync(a => a.AnnotationId == entity.AnnotationId);

            return added.ToModel();
        }

        private Expression<Func<Annotation, bool>> GetTagFilterExpression(IEnumerable<int> tagFilters)
        {
            Expression expression = null;
            const string propertyName = "TagId";
            var parameterExpression = Expression.Parameter(typeof(AnnotationTag), "t");
            var memberExpression = Expression.Property(parameterExpression, propertyName);
            foreach (var id in tagFilters)
            {
                var valExp = Expression.Constant(id, typeof(int));
                var exp = Expression.Equal(memberExpression, valExp);
                if (expression == null)
                {
                    expression = exp;
                }
                else
                {
                    expression = Expression.OrElse(expression, exp);

                }
            }

            var anyInnerExpression = Expression.Lambda<Func<AnnotationTag, bool>>(expression, parameterExpression);

            parameterExpression = Expression.Parameter(typeof(Annotation), "a");
            memberExpression = Expression.Property(parameterExpression, "Tags");
            var callExpression = memberExpression.CallAny(anyInnerExpression);
            var rt = Expression.Lambda<Func<Annotation, bool>>(callExpression, parameterExpression);

            return rt;

        }


        private static Expression<Func<Annotation, bool>> GetDateFilterExpression(IEnumerable<DateTime> dateFilters)
        {
            Expression expression = null;
            const string propertyName = "CreatedDate";
            var parameterExpression = Expression.Parameter(typeof(Annotation), "a");
            var memberExpression = Expression.Property(parameterExpression, propertyName);
            memberExpression = Expression.Property(memberExpression, "Date");
            foreach (var dt in dateFilters) {
                var valExp = Expression.Constant(dt.Date, typeof(DateTime));
                var exp = Expression.Equal(memberExpression, valExp);
                if (expression == null)
                {
                    expression = exp;
                }
                else
                {
                    expression = Expression.OrElse(expression, exp);

                }
            }

            var rt = Expression.Lambda<Func<Annotation, bool>>(expression, parameterExpression);

            return rt;

        }

        private static Expression<Func<Annotation, bool>> GetTypeFilterExpression(IEnumerable<int?> typefilters)
        {        
            Expression expression = null;
            const string propertyName = "AnnotationTypeId";
            var parameterExpression = Expression.Parameter(typeof(Annotation), "a");
            var memberExpression = Expression.Property(parameterExpression, propertyName);
            foreach (var id in typefilters) {
                var valExp = Expression.Constant(id, typeof(int?));
                var exp = Expression.Equal(memberExpression, valExp);
                if (expression == null)
                {
                    expression = exp;
                }
                else
                {
                    expression = Expression.OrElse(expression, exp);

                }
            }

            var rt = Expression.Lambda<Func<Annotation, bool>>(expression, parameterExpression);

            return rt;
        }


        private static Expression<Func<Annotation, bool>> GetPersonFilterExpression(IEnumerable<int> personFilters) {
            
            Expression expression = null;
            const string propertyName = "CreatedUserId";

            var parameterExpression = Expression.Parameter(typeof(Annotation), "a");
            var memberExpression = Expression.Property(parameterExpression, propertyName);

            foreach (var id in personFilters) {
                var valExp = Expression.Constant(id, typeof(int));
                var exp = Expression.Equal(memberExpression, valExp);
                if (expression == null)
                {                   
                    expression = exp;
                }
                else
                {
                    expression = Expression.OrElse(expression, exp);
              
                }
                
            }

            var rt = Expression.Lambda<Func<Annotation, bool>>(expression, parameterExpression);

            return rt;
         
        }
        
    }
}
