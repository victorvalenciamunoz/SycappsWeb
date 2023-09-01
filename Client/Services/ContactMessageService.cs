using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;
using System.Net.Http.Json;

namespace SycappsWeb.Client.Services;

public class ContactMessageService : IContactMessageService
{
    private readonly HttpClient httpClient;

    public ContactMessageService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task SendMessage(ContactMessageRequest message)
    {
        var result = await httpClient.PostAsJsonAsync<ContactMessageRequest>("api/ContactMessage/post", message);
    }

    public async Task<PagedResponse<IEnumerable<MensajeContacto>>> GetAll(int pageNumber, int pageSize)
    {
        var pagedResult =  await httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<MensajeContacto>>>($"api/ContactMessage/list?PageNumber={pageNumber}&PageSize={pageSize}");
        if (pagedResult != null)
        {
            return pagedResult;
        }

        return null;
    }
    public async Task<PagedResponse<IEnumerable<MensajeContacto>>> GetUnread(int pageNumber, int pageSize)
    {        
        var pagedResult = await httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<MensajeContacto>>>($"api/ContactMessage/unread?PageNumber={pageNumber}&PageSize={pageSize}");
        if (pagedResult != null)
        {
            return pagedResult;
        }

        return null;
    }

    public async Task MarkAsRead(int id)
    {
        await httpClient.PutAsJsonAsync<int>($"api/ContactMessage/{id}/read", 0);
    }
}
