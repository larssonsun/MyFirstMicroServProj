using System.Threading.Tasks;

namespace Client.Con
{
    public interface IScetiService
    {
        void GetServices();
        Task<string> CreateConsign();
        Task<string> GetConsigns();
    }
}