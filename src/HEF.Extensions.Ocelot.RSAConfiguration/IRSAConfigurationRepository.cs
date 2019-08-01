using Ocelot.Responses;
using System.Threading.Tasks;

namespace HEF.Extensions.Ocelot.RSAConfiguration
{
    public interface IRSAConfigurationRepository
    {
        Task<Response<string>> GetPublicKey();

        Task<Response<string>> GetPrivateKey();
    }
}
