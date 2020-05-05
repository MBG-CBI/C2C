using System;
using System.Threading.Tasks;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Interfaces
{
    public interface ITokenProvider
    {
        Task<GetAccessTokenResponse> GetNewAccessTokenAsync(GetAccessTokenRequest request);
        bool IsTokenExpired(string accessToken);
        DateTime GetTokenExpiration(string accessToken);
    }
}
