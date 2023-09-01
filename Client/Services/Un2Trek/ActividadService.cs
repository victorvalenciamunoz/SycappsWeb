using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;
using System.Net.Http.Json;
using SycappsWeb.Shared.ExtensionMethods;

namespace SycappsWeb.Client.Services.Un2Trek;

public class ActividadService : IActividadService
{
    private readonly HttpClient httpClient;

    public ActividadService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ActividadResponse> Get(int activityId)
    {
        var activity = await httpClient.GetFromJsonAsync<ActividadResponse>($"api/v1/Un2TrekActividad/{activityId}");
        if (activity != null)
        {
            return activity;
        }
        return null;

    }

    public async Task<PagedResponse<IEnumerable<ActividadResponse>>> GetAll(int pageNumber, int pageSize)
    {
        var pagedResult = await httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<ActividadResponse>>>($"api/v1/Un2TrekActividad/all?PageNumber={pageNumber}&PageSize={pageSize}");
        if (pagedResult != null)
        {
            return pagedResult;
        }
        return null;
    }

    public async Task<IEnumerable<TrekiResponse>> GetActivityTrekiList(int activityId)
    {
        var trekiList = await httpClient.GetFromJsonAsync<IEnumerable<TrekiResponse>>($"api/v1/Un2TrekActividad/{activityId}/TrekiList");
        if (trekiList != null)
        {
            return trekiList;
        }
        return null;
    }

    public async Task<IEnumerable<TrekiResponse>> GetUnassignedTrekiList(int activityId)
    {
        var trekiList = await httpClient.GetFromJsonAsync<IEnumerable<TrekiResponse>>($"api/v1/Un2TrekActividad/{activityId}/unassignedTrekiList");
        if (trekiList != null)
        {
            return trekiList;
        }
        return null;

    }

    public async Task Update(ActividadResponse actividadResponse)
    {
        var updatedActivity = actividadResponse.ToRequest();
        if (actividadResponse.Id != 0)
        {
            await httpClient.PutAsJsonAsync<ActividadRequest>($"api/v1/Un2TrekActividad/{actividadResponse.Id}/update", updatedActivity);
        }
        else
        {
            await httpClient.PostAsJsonAsync<ActividadRequest>($"api/v1/Un2TrekActividad", updatedActivity);
        }
    }

    public async Task AssignTrekiToActivity(int activityId, int trekiId)
    {
        await httpClient.PostAsJsonAsync<int>($"api/v1/Un2TrekActividad/{activityId}/treki", trekiId);
    }    
    public async Task RemoveTrekiFromActivity(int activityId, int trekiId)
    {
        await httpClient.DeleteAsync($"api/v1/Un2TrekActividad/{activityId}/treki/{trekiId}");
    }
}
