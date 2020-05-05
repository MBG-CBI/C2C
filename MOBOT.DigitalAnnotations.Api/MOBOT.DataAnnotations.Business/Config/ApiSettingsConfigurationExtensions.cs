using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace MOBOT.DigitalAnnotations.Business.Config
{
    public static class ApiSettingsConfigurationExtensions
    {
        public static IConfigurationBuilder AddApiConfiguration(this IConfigurationBuilder builder, IHostingEnvironment hostingEnvironment)
        {
            return builder.Add(new ApiSettingsConfigurationSource(hostingEnvironment));
        }
    }
}
