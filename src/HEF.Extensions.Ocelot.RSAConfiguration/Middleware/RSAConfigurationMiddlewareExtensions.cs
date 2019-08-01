using Ocelot.Middleware.Pipeline;

namespace HEF.Extensions.Ocelot.RSAConfiguration
{
    internal static class RSAConfigurationMiddlewareExtensions
    {
        internal static IOcelotPipelineBuilder UseRSAConfiguration(this IOcelotPipelineBuilder builder, string clientRequestPath)
        {
            return builder.UseMiddleware<RSAConfigurationMiddleware>(clientRequestPath);
        }
    }
}
