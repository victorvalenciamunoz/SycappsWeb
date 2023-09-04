using SycappsWeb.Shared.Entities.Un2Trek;

namespace SycappsWeb.Server.Data;

public interface ITrekiRepository
{
    Task Add(Treki newTreki);
    Task<Treki?> GetById(int id);
    Task<List<Treki>> GetTrekiListByActivity(int activityId);
    
    Task<List<Treki>> GetTrekiListNotInActivity(int activityId);
    
    Task<List<Treki>> GetTrekisAround(double currentLatitude, double currentLongitude, string threshold);
    
    Task<bool> Modify(Treki trekiToModify);
    
    Task<Treki> GetTrekiByCoordinates(double latitude, double longitude);
    
    Task CaptureTreki(Treki treki, string userId, int activityId);

    bool IsTrekiAlreadyCaptured(int trekiId, string userId, int activityId);
}