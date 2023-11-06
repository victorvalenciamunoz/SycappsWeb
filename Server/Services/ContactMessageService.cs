using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Services;

public class ContactMessageService : IContactMessageService
{
    private readonly ApplicationDbContext context;

    public ContactMessageService(ApplicationDbContext context)
    {
        this.context = context;
    }
    public Task Add(MensajeContacto message)
    {
        throw new Exception();
        context.Add(message);
        return context.SaveChangesAsync();
    }

    public async Task<PagedResponse<List<MensajeContacto>>> All(PaginationFilter pagination)
    {
        PagedResponse<List<MensajeContacto>> result;

        var totalRecords = await context.MensajesContacto.CountAsync();

        var messages = await context.MensajesContacto
            .OrderBy(m => m.FechaRecepcion)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize).ToListAsync();

        result = new PagedResponse<List<MensajeContacto>>(messages, pagination.PageNumber, pagination.PageSize, totalRecords);

        return result;
    }
    public async Task<PagedResponse<List<MensajeContacto>>> Unread(PaginationFilter pagination)
    {
        PagedResponse<List<MensajeContacto>> result;

        var totalRecords = await context.MensajesContacto.Where(c => c.Leido == false).CountAsync();

        var messages = await context.MensajesContacto
            .Where(c => c.Leido == false)
            .OrderBy(m => m.FechaRecepcion)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize).ToListAsync();

        result = new PagedResponse<List<MensajeContacto>>(messages, pagination.PageNumber, pagination.PageSize, totalRecords);

        return result;
    }
    public async Task MarkAsRead(int id)
    {
        var message = await context.MensajesContacto.Where(c => c.Id == id).FirstOrDefaultAsync();
        if (message != null)
        {
            message.FechaLeido = System.DateTime.Now;
            message.Leido = true;

            await context.SaveChangesAsync();
        }
    }
}
