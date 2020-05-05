using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MOBOT.DigitalAnnotations.Business.Interfaces;
using MOBOT.DigitalAnnotations.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MOBOT.DigitalAnnotations.Business.Services
{
    public class RerumCommunicatorService : IRerumCommunicator
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<RerumCommunicatorService> _logger;
        private readonly ITokenManager<RerumToken> _tokenManager;
        private string _rerumApiUrl; 

        public RerumCommunicatorService(ITokenManager<RerumToken> tokenManager, IConfigurationRoot config, ILogger<RerumCommunicatorService> logger)
        {
            _tokenManager = tokenManager;
            _config = config;
            _logger = logger;
            _rerumApiUrl = _config["RerumApiUrl"];
        }

        public async Task<RerumManifestResponse> PostManifest(WebManifest manifest)
        {
            _logger.LogDebug($"Posting manifest: Context: {manifest.Context}, id: {manifest.Id}.");
            
            return await MakeRequest(manifest, "post");
        }

        public async Task<RerumManifestResponse> PutManifest(WebManifest manifest)
        {
            _logger.LogDebug($"Putting manifest: Context: {manifest.Context}, id: {manifest.Id}.");
            
            return await MakeRequest(manifest, "put");
        }

        private async Task<RerumManifestResponse> MakeRequest(WebManifest manifest, string method)
        {           
            var rt = new RerumManifestResponse();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders
                    .Add("Accept", new[] { Constants.MediaTypes.ApplicationJSon, Constants.MediaTypes.ApplicationJSonLD });

                var authToken = await _tokenManager.GetTokenAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);
                var jsonSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        },
                        NullValueHandling = NullValueHandling.Ignore
                    };

                var json = JsonConvert.SerializeObject(manifest, jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, Constants.MediaTypes.ApplicationJSon);
                HttpResponseMessage response = null;
                string url = null;
                if (method.Equals("post", StringComparison.InvariantCultureIgnoreCase))
                {
                    url = $"{_rerumApiUrl}/api/create.action";
                    response = await client.PostAsync(url, content);  // await client.PostAsJsonAsync(url, manifest);
                }
                else  if (method.Equals("put", StringComparison.InvariantCultureIgnoreCase)) {
                    url = $"{_rerumApiUrl}/api/update.action";
                    response = await client.PutAsync(url, content);
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    _logger.LogDebug($"Manifest: Context: {manifest.Context}, id: {manifest.Id} post\\update successful.");                    
                    rt = JsonConvert.DeserializeObject<RerumManifestResponse>(jsonResponse, jsonSettings);
                }
                else
                {
                    _logger.LogWarning($"Unable to post\\update manifest: Context: {manifest.Context}, id: {manifest.Id}.");
                }
            }

            return rt;
        }
    }
}
