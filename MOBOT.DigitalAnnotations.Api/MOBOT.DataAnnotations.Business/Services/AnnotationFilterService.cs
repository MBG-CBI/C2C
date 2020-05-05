using Microsoft.EntityFrameworkCore;

using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Enumerations;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class AnnotationFilterService : IAnnotationFilterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Annotation> _annotations;

        public AnnotationFilterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _annotations = _unitOfWork.Set<Annotation>();
        }

        public async Task<IEnumerable<AnnotationFilterTypeModel>> GetFiltersForSource(int sourceId)
        {
            var list = new List<AnnotationFilterTypeModel>();
            var pfs = await _annotations
                .Include(a => a.Target)
                .Include(a => a.CreatedUser)
                .Where(a => a.Target.AnnotationSourceId == sourceId)
                .GroupBy(a => a.CreatedUser)
                .Select(g => new { count = g.Count(), user = g.Key.Email, id = g.Key.Id })
                .OrderByDescending(g => g.count).ThenBy(g => g.user)
                .ToListAsync();

            if (pfs.Count > 0)
            {
                var filterType = new AnnotationFilterTypeModel
                {
                    Type = AnnotationFilterTypes.Person,
                    Filters = pfs.ConvertAll(pf => new AnnotationFilterModel {
                        Count = pf.count,
                        Name = pf.user,
                        Id = pf.id
                    })
                };
                list.Add(filterType);
            }

            var typs = await _annotations
                .Where(a => a.Target.AnnotationSourceId == sourceId)
                .GroupBy(a => a.AnnotationType)
                .Select(g => new { count = g.Count(), name = g.Key != null ? g.Key.TypeName : "No Type", id = g.Key != null ? g.Key.AnnotationTypeId : (int?)null })
                .OrderByDescending(g => g.count).ThenBy(g => g.name)
                .ToListAsync();


            if (typs.Count > 0) {
                var filterType = new AnnotationFilterTypeModel {
                    Type = AnnotationFilterTypes.Type,
                    Filters = typs.ConvertAll(t => new AnnotationFilterModel {
                        Count = t.count,
                        Id = t.id,
                        Name = t.name
                    })
                };
                list.Add(filterType);
            }

            var dts = await _annotations
                .Where(a => a.Target.AnnotationSourceId == sourceId)
                .GroupBy(a => a.CreatedDate.Date)
                .Select(g => new { count = g.Count(), date = g.Key, id = 0 })
                .OrderByDescending(g => g.count).ThenBy(g => g.date)
                .ToListAsync();
            if (dts.Count > 0) {
                var filterType = new AnnotationFilterTypeModel
                {
                    Type = AnnotationFilterTypes.Date,
                    Filters = dts.ConvertAll(d => new AnnotationFilterModel
                    {
                        Count = d.count,
                        Id = d.id,
                        Name = d.date.ToString("MM/dd/yyyy")
                    })
                };
                list.Add(filterType);
            }

            var tgs = await _annotations
                .Where(a => a.Target.AnnotationSourceId == sourceId)
                .SelectMany(a => a.Tags)
                .GroupBy(a => a.Tag)
                .Select(g => new { count = g.Count(), name = g.Key.TagName, id = g.Key.TagId })
                .OrderByDescending(g => g.count).ThenBy(g => g.name)
                .ToListAsync();
            if (tgs.Count > 0) {
                var filterType = new AnnotationFilterTypeModel
                {
                    Type = AnnotationFilterTypes.Tags,
                    Filters = tgs.ConvertAll(t => new AnnotationFilterModel
                    {
                        Count = t.count,
                        Id = t.id,
                        Name = t.name
                    })
                };
                list.Add(filterType);
            }


            return list;
        }
    }
}
