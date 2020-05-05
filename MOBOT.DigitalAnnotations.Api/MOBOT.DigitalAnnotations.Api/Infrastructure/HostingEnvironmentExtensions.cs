using Microsoft.AspNetCore.Hosting;

namespace MOBOT.DigitalAnnotations.Api.Infrastructure
{
    public static class HostingEnvironmentExtensions
    {
        public const string TestEnvironment = "Test";

        public static bool IsTest(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment != null && hostingEnvironment.IsEnvironment(TestEnvironment);
        }
    }
}
