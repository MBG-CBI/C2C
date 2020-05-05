using MOBOT.DigitalAnnotations.Domain.Models;
using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface IAuthorizationService
    {
        Task<UserModel> LoginAsync(string userName, string password);
    }
}
