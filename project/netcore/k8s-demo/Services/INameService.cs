
using System.Threading.Tasks;

namespace k8s_demo.Services
{
    public interface INameService
    {
        Task<string> GetName();
    }
}