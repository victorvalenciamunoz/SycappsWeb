using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Server.Services;

public interface IActividadService
{
    Task<ActividadResponse> Get(int id);
    Task<Actividad> Add(Actividad newActividad);
    Task Update(int id, ActividadRequest updatedActivity);
    Task<IEnumerable<ActividadResponse>> GetActiveActivityList(DateTime currentDate);
    Task<PagedResponse<IEnumerable<ActividadResponse>>> All(PaginationFilter pagination);
    Task AssignTrekiToActivity(int activityId, int trekiId);
    Task RemoveTrekiFromActivity(int activityId, int trekiId);
}