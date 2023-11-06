using ErrorOr;
using SycappsWeb.Server.Models.IntegrationModels;
using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Server.Services;

public interface ITrekiService
{
    Task Add(TrekiDto newTreki);

    Task<ErrorOr<Success>> Capture(CaptureTrekiRequest capture, string userId);

    Task<List<TrekiDto>> GetTrekisAround(double currentLatitude, double currentLongitude);

    Task<ErrorOr<Updated>> Modify(TrekiDto trekiToModify);

    Task<ErrorOr<Deleted>> Delete(int id);

    Task<List<TrekiDto>> GetTrekiListByActivity(int activityId);

    Task<List<TrekiDto>> GetTrekiListNotInActivity(int activityId);
}