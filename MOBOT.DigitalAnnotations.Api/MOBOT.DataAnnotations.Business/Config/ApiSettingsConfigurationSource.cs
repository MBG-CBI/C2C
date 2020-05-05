using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace MOBOT.DigitalAnnotations.Business.Config
{
    public class ApiSettingsConfigurationSource : FileConfigurationSource
    {
        private readonly IHostingEnvironment _environment;

        public ApiSettingsConfigurationSource(IHostingEnvironment environment)
        {
            _environment = environment;
            Path = _environment != null ? $"apisettings.{_environment.EnvironmentName}.json" : "apisettings.json";
            ReloadOnChange = true;
            Optional = true;
            FileProvider = null;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new ApiSettingsConfigurationProvider(this);
        }
    }
}
