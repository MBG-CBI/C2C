using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AnnotationTargetService : IAnnotationTargetService
    {
        private readonly ILogger<AnnotationTargetService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AnnotationTarget> _targets;
        private readonly DbSet<Annotation> _annotations;

        public AnnotationTargetService(IUnitOfWork unitOfWork, ILogger<AnnotationTargetService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _targets = _unitOfWork.Set<AnnotationTarget>();
            _annotations = _unitOfWork.Set<Annotation>();
        }


        public async Task<IEnumerable<AnnotationModel>> GetAnnotations(long id)
        {
            List<AnnotationModel> list = null;
            var annotations = await _annotations.Where(a=>a.AnnotationTargetId == id && a.ParentId == null)
                .Include(a => a.Target)
                .Include(a => a.License)
                .Include(a => a.Group)
                .Include(a => a.CreatedUser)
                .Include(a => a.UpdatedUser)
                .Include(a => a.AnnotationType)
                .Include(a => a.Annotations)
                .Include(a => a.Tags).ThenInclude(at => at.Tag)
                .ToListAsync();
            list = annotations.ConvertAll(a => a.ToModel());
            return list;
        }
    }
}
