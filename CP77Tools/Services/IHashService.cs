using System.Threading.Tasks;

namespace CP77Tools.Services
{
    public interface IHashService
    {
        Task<bool> RefreshAsync();
    }
}