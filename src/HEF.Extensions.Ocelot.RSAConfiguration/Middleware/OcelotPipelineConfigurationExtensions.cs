using Ocelot.Middleware;
using System;

namespace HEF.Extensions.Ocelot.RSAConfiguration
{
    public static class OcelotPipelineConfigurationExtensions
    {
        public static OcelotPipelineConfiguration UseRSAConfiguration(this OcelotPipelineConfiguration configuration,
            string clientRequestPath)
        {
            ValidatePipelineMapPath(clientRequestPath);

            configuration.MapWhenOcelotPipeline.Add(app =>
            {
                app.UseRSAConfiguration(clientRequestPath);

                return context => context.HttpContext.Request.Path.StartsWithSegments(clientRequestPath);
            });

            return configuration;
        }

        private static void ValidatePipelineMapPath(string requestPath)
        {
            if (string.IsNullOrWhiteSpace(requestPath))
                throw new ArgumentNullException(nameof(requestPath));

            if (requestPath.Contains("?"))
                throw new ArgumentException($"{nameof(requestPath)} cannot contain query string values");

            if (!requestPath.StartsWith("/"))
                throw new ArgumentException($"{nameof(requestPath)} should start with '/'");
        }
    }
}
