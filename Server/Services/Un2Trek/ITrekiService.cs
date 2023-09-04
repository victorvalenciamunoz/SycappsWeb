using SycappsWeb.Server.Models.IntegrationModels;
using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Server.Services;

public interface ITrekiService
{
    Task Add(TrekiDto newTreki);

    Task<ServiceResultSingleElement<bool>> Capture(CaptureTrekiRequest capture, string userId);

    Task<List<TrekiDto>> GetTrekisAround(double currentLatitude, double currentLongitude);

    Task<ServiceResultSingleElement<bool>> Modify(TrekiDto trekiToModify);

    Task<ServiceResultSingleElement<bool>> Delete(int id);

    Task<List<TrekiDto>> GetTrekiListByActivity(int activityId);

    Task<List<TrekiDto>> GetTrekiListNotInActivity(int activityId);
}