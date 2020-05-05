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
    public class LicenseService : ILicenseService
    {
        private readonly ILogger<LicenseService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<License> _licenses;

        public LicenseService(IUnitOfWork unitOfWork, ILogger<LicenseService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _licenses = _unitOfWork.Set<License>();
        }

        public async Task<IEnumerable<LicenseModel>> GetListAsync()
        {
            var list = await _licenses.OrderBy(l => l.Sequence)
                .ToListAsync();
            return list.ConvertAll(l => l.ToModel());
        }
    }
}
