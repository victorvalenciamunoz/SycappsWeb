using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Client.Services.Un2Trek;

public interface IActividadService
{
    Task<ActividadResponse> Get(int activityId);
    Task<PagedResponse<IEnumerable<ActividadResponse>>> GetAll(int pageNumber, int pageSize);
    Task Update(ActividadResponse actividadResponse);
    Task<IEnumerable<TrekiResponse>> GetActivityTrekiList(int activityId);
    Task<IEnumerable<TrekiResponse>> GetUnassignedTrekiList(int activityId);
    Task AssignTrekiToActivity(int activityId, int trekiId);
    Task RemoveTrekiFromActivity(int activityId, int trekiId);
}