
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace k8s_demo.Services
{
    public class NameService : INameService
    {
        private readonly string _nameUrl="http://name-api/api/name";
        private readonly ILogger<NameService> _logger;

        public NameService(ILogger<NameService> logger)
        {
            _logger=logger;
        }

        public async Task<string> GetName()
        {
            return await _nameUrl.GetStringAsync(); 
        }
    }
}