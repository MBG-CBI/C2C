using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MOBOT.DigitalAnnotations.Business.Config;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class TokenManagerService : ITokenManager<RerumToken>
    {
        private readonly IMemoryCache _cache;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfigurationRoot _settings;
        private string _currentRerumRefreshToken;

        public TokenManagerService(ITokenProvider tokenProvider, IConfigurationRoot settings, IMemoryCache cache)
        {
            _tokenProvider = tokenProvider;
            _settings = settings;
            _cache = cache;
            _currentRerumRefreshToken = _settings["RerumRefreshToken"];
        }

        public async Task<RerumToken> GetTokenAsync()
        {
            return await GetRerumAccessTokenAsync();
        }

        private async Task<RerumToken> GetRerumAccessTokenAsync()
        {
            RerumToken token = null;
            if (!_cache.TryGetValue("RerumToken", out token))
            {
                var request = new GetAccessTokenRequest
                {
                    RefreshToken = _currentRerumRefreshToken
                };
                var tokenResponse = await _tokenProvider.GetNewAccessTokenAsync(request);
                if (tokenResponse.IsSuccess)
                {
                    token = new RerumToken { AccessToken = tokenResponse.AccessToken, IdToken = tokenResponse.IdToken, RefreshToken = tokenResponse.RefreshToken };
                    var span = _tokenProvider.GetTokenExpiration(token.AccessToken) - DateTime.UtcNow;
                    _currentRerumRefreshToken = token.RefreshToken;
                    _settings["RerumApiToken"] = tokenResponse.AccessToken;
                    // Don't need to set the Refresh token as it does not expire? _settings["RerumRefreshToken"] = tokenResponse.RefreshToken;
                    var settingsConfigurationProvider = _settings.Providers.FirstOrDefault(p => p.GetType() == typeof(ApiSettingsConfigurationProvider));
                    var opts = new MemoryCacheEntryOptions().SetAbsoluteExpiration(span);
                    _cache.Set("RerumToken", token, opts);
                }
            }

            return token;
        }
    }
}
