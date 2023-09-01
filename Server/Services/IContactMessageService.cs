using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Services;

public interface IContactMessageService
{
    Task Add(MensajeContacto message);
    Task<PagedResponse<List<MensajeContacto>>> All(PaginationFilter pagination);
    Task<PagedResponse<List<MensajeContacto>>> Unread(PaginationFilter pagination);
    Task MarkAsRead(int id);
}