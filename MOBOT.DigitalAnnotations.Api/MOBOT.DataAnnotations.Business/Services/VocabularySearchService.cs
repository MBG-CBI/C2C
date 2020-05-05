using Microsoft.EntityFrameworkCore;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;
using MOBOT.DigitalAnnotations.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class VocabularySearchService : IVocabularySearchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<VocabularyListView> _terms;

        public VocabularySearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _terms = _unitOfWork.Set<VocabularyListView>();
        }

        public async Task<IEnumerable<VocabularyLookupResponseModel>> Lookup(VocabularyLookupRequestModel request)
        {
            var results = await _terms
                .Where(t => EF.Functions.Like(t.SearchTerm, $"{request.SearchTerm}%"))
                .OrderBy(t => t.Term)
                .Take(10)
                .ToListAsync();

            var list = results.ConvertAll(t => new VocabularyLookupResponseModel
            {
                SearchTerm = t.SearchTerm,
                Term = t.Term
            });

            return list;
        }
    }
}
