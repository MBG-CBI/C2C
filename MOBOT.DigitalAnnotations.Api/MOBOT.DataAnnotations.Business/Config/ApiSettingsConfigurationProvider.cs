using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using MOBOT.DigitalAnnotations.Business.Models;
using Newtonsoft.Json;

namespace MOBOT.DigitalAnnotations.Business.Config
{
    public class ApiSettingsConfigurationProvider : FileConfigurationProvider
    {        
        public ApiSettingsConfigurationProvider(ApiSettingsConfigurationSource source) : base (source)  {  }

        public override void Load(Stream stream)
        {
            try
            {
                Data = ReadApiSettings(stream);
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred trying to load api configuration from file {Source.Path}");
            }
        }

        public void SaveSettings()
        {
            
            var settings = new ApiSettings();
            settings.ApiBaseUrl = Data[nameof(settings.ApiBaseUrl)];
            settings.RerumRefreshToken = Data[nameof(settings.RerumRefreshToken)];
            settings.RerumApiToken = Data[nameof(settings.RerumApiToken)];
            settings.RerumApiUrl = Data[nameof(settings.RerumApiUrl)];
            settings.RerumAuthCode = Data[nameof(settings.RerumAuthCode)];

            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(Source.Path, json);
        }

        private IDictionary<string, string> ReadApiSettings(Stream stream)
        {
            var serializer = new JsonSerializer();
            ApiSettings settings = null;
            var data =
                new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.StartObject)
                    {
                        settings = serializer.Deserialize<ApiSettings>(jr);
                    }
                }

            }

            if (settings != null)
            {
                data[nameof(settings.ApiBaseUrl)] = settings.ApiBaseUrl;
                data[nameof(settings.RerumRefreshToken)] = settings.RerumRefreshToken;
                data[nameof(settings.RerumApiToken)] = settings.RerumApiToken;
                data[nameof(settings.RerumApiUrl)] = settings.RerumApiUrl;
                data[nameof(settings.RerumAuthCode)] = settings.RerumAuthCode;
            }

            return data;
        }
    }
}
