using SycappsWeb.Shared.Entities.Un2Trek;

namespace SycappsWeb.Server.Data
{
    public interface IActivityRepository
    {
        Task<Actividad> GetById(int id);
    }
}