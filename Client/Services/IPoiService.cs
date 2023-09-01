using SycappsWeb.Shared.Entities;

namespace SycappsWeb.Client.Services
{
    public interface IPoiService
    {
        Task Import(List<PuntodeInteres> data);
    }
}