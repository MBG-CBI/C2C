using MOBOT.DigitalAnnotations.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface ILicenseService
    {
        Task<IEnumerable<LicenseModel>> GetListAsync();
    }
}
