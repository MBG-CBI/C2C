using System.Threading.Tasks;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface ITokenManager<T>
    {
        Task<T> GetTokenAsync();
    }
}
