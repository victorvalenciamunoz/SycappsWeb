using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Services
{
    public interface IPoiService
    {
        Task<List<PuntoDto>> Get(double latitude, double longitude);
        Task Import(List<PuntodeInteres> puntos);
        Task<List<PuntodeInteres>> Prueba();
    }
}