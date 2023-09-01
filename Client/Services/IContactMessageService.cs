using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Client.Services;

public interface IContactMessageService
{
    Task SendMessage(ContactMessageRequest message);
    Task<PagedResponse<IEnumerable<MensajeContacto>>> GetAll(int pageNumber, int pageSize);
    Task<PagedResponse<IEnumerable<MensajeContacto>>> GetUnread(int pageNumber, int pageSize);
    Task MarkAsRead(int id);
}