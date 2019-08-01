using Microsoft.AspNetCore.Http;
using Ocelot.DownstreamRouteFinder.Finder;
using Ocelot.Logging;
using Ocelot.Middleware;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HEF.Extensions.Ocelot.RSAConfiguration
{
    public class RSAConfigurationMiddleware : OcelotMiddleware
    {
        private readonly OcelotRequestDelegate _next;

        private readonly IRSAConfigurationRepository _rsaConfigRepo;

        private readonly string _clientRequestPath;
        private readonly string _rsaPublicRequestPath;

        public RSAConfigurationMiddleware(OcelotRequestDelegate next,
            IOcelotLoggerFactory loggerFactory,
            IRSAConfigurationRepository rsaConfigRepo,
            string clientRequestPath)
            : base(loggerFactory.CreateLogger<RSAConfigurationMiddleware>())
        {
            _next = next;

            _rsaConfigRepo = rsaConfigRepo;

            _clientRequestPath = clientRequestPath;
            _rsaPublicRequestPath = $"{clientRequestPath}/public";
        }

        public async Task Invoke(DownstreamContext context)
        {
            if (!IsRSAPublicKeyRequest(context.HttpContext))
            {
                var request = context.HttpContext.Request;
                SetPipelineError(context, new UnableToFindDownstreamRouteError(request.Path, request.Method));
                return;
            }
           
            var publicKeyResult = await _rsaConfigRepo.GetPublicKey();

            if (publicKeyResult == null || string.IsNullOrWhiteSpace(publicKeyResult.Data))
            {
                context.DownstreamResponse = BuildDownstreamResponse(HttpStatusCode.NotFound, "not found rsa public key.");
                return;
            }

            context.DownstreamResponse = BuildDownstreamResponse(HttpStatusCode.OK, publicKeyResult.Data);
        }

        private bool IsRSAPublicKeyRequest(HttpContext context)
        {
            return context.Request.Path == _rsaPublicRequestPath;
        }

        private static DownstreamResponse BuildDownstreamResponse(HttpStatusCode statusCode, string contentStr)
        {
            return new DownstreamResponse(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(contentStr)
            });
        }
    }
}
