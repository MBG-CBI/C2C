using Microsoft.EntityFrameworkCore;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Converters;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Tag> _tags;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tags = _unitOfWork.Set<Tag>();
        }

        public async Task<IEnumerable<TagModel>> ListAsync()
        {
            var tags = await _tags.ToListAsync();
            var list = tags.ConvertAll(t => t.ToModel());
            return list;
        }
    }
}
