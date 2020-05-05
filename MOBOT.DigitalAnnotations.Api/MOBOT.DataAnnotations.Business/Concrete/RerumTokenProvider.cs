using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;
using MOBOT.DigitalAnnotations.Business.Services;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Concrete
{
    public class RerumTokenProvider : ITokenProvider
    {
        private readonly ILogger<RerumTokenProvider> _logger;
        private readonly IConfigurationRoot _appSettings;
        private readonly string _url;

        public RerumTokenProvider(IConfigurationRoot appSettings, ILogger<RerumTokenProvider> logger)
        {
            _logger = logger;
            _appSettings = appSettings;
            _url = _appSettings["RerumApiUrl"];
        }

        public async Task<GetAccessTokenResponse> GetNewAccessTokenAsync(GetAccessTokenRequest request)
        {
            _logger.LogDebug($"Attempting to get access token using authToken: {request.RefreshToken}. Rerum api url: {_url}.");
            var rt = new GetAccessTokenResponse { IsSuccess = true };
            using (var client = new HttpClient())
            {
                
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));                
               
                var response = await client.PostAsJsonAsync($@"{_url}/api/accessToken.action", new { refresh_token = request.RefreshToken });
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogDebug($"Access token retrieved successful.");
                    var respString = await response.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(respString);
                    rt.AccessToken = obj.access_token;
                    rt.IdToken = obj.id_token;                   

                    return rt;
                } else
                {
                    var message = $"An error occurred retrieving access token from Rerum. Status Code: {response.StatusCode}, Error: {response.ReasonPhrase}.";
                    _logger.LogError(message);
                    rt.IsSuccess = false;
                    rt.Message = message;
                }
            }
            return rt;
        }

        public DateTime GetTokenExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadToken(token) as JwtSecurityToken;
            return jwt.ValidTo;
        }

        public bool IsTokenExpired(string token)
        {
            return GetTokenExpiration(token) >= DateTime.UtcNow;
        }
    }
}
