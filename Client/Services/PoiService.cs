using SycappsWeb.Shared.Entities;
using System.Net.Http.Json;

namespace SycappsWeb.Client.Services;

public class PoiService : IPoiService
{
    private readonly HttpClient httpClient;

    public PoiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Import(List<PuntodeInteres> data)
    {
        var result = await httpClient.PostAsJsonAsync<List<PuntodeInteres>>("api/puntosdeinteres/import", data);
    }
}
