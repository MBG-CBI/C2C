using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AnnotationTypeService : IAnnotationTypeService
    {
        private readonly ILogger<AnnotationTypeService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<AnnotationType> _annotationTypes;

        public AnnotationTypeService(IUnitOfWork unitOfWork, ILogger<AnnotationTypeService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _annotationTypes = _unitOfWork.Set<AnnotationType>();
        }

        public async Task<IEnumerable<AnnotationTypeModel>> GetListAsync()
        {
            var types = await _annotationTypes.OrderBy(a => a.TypeName).ToListAsync();
            var list = types.ConvertAll(at => at.ToModel());

            return list;
        }
    }
}
