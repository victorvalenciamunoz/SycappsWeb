using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models;
using SycappsWeb.Shared.Models.Un2Trek;
using SycappsWeb.Shared.ExtensionMethods;

namespace SycappsWeb.Server.Services;

public class ActividadService : IActividadService
{
    private readonly ApplicationDbContext context;

    public ActividadService(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    public async Task<ActividadResponse> Get(int id)
    {
        var actividad = await context.Actividades.Where(c=> c.Id == id).FirstOrDefaultAsync();
        if (actividad!=null)
            return actividad.ToResponse();

        return null;
    }
    public async Task<Actividad> Add(Actividad newActividad)
    {
        context.Add(newActividad);
        await context.SaveChangesAsync();
        return newActividad;
    }

    public async Task<IEnumerable<ActividadResponse>> GetActiveActivityList(DateTime currentDate)
    {
        var actividades = await context.Actividades
                        .Where(c => (c.ValidoHasta != null && c.ValidoHasta >= currentDate) || c.ValidoHasta == null).ToListAsync();
        if (actividades.Any())
        {
            List<ActividadResponse> listaActividades = new List<ActividadResponse>();
            foreach (var actividad in actividades)
            {
                listaActividades.Add(actividad.ToResponse());
            }

            return listaActividades;
        }
        else
        {
            return Enumerable.Empty<ActividadResponse>();
        }
    }

    public async Task<PagedResponse<IEnumerable<ActividadResponse>>> All(PaginationFilter pagination)
    {
        var actividades = await context.Actividades.ToListAsync();
        if (actividades.Any())
        {
            List<ActividadResponse> listaActividades = new List<ActividadResponse>();
            foreach (var actividad in actividades)
            {
                listaActividades.Add(actividad.ToResponse());
            }

            var filtered = listaActividades
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize);
            return new PagedResponse<IEnumerable<ActividadResponse>>(filtered, pagination.PageNumber, pagination.PageSize, listaActividades.Count());
        }
        else
        {
            return new PagedResponse<IEnumerable<ActividadResponse>>(Enumerable.Empty<ActividadResponse>(), pagination.PageNumber, pagination.PageSize, 0);
        }
    }   
    
    public async Task Update(int activityId, ActividadRequest updatedActivity)
    {
        var actividad = await context.Actividades.Where(c => c.Id == activityId).FirstOrDefaultAsync();
        if (actividad != null)
        {
            actividad!.Titulo = updatedActivity.Titulo;
            actividad!.ValidoDesde = updatedActivity.ValidoDesde;
            actividad!.ValidoHasta = updatedActivity.ValidoHasta;
            await context.SaveChangesAsync();
        }
    }

    public async Task AssignTrekiToActivity(int activityId, int trekiId)
    {
        
        var actividad = await context.Actividades
                        .Include(c => c.Treki)
                        .Where(c => c.Id == activityId).FirstOrDefaultAsync();
        var treki = await context.Trekis.Where(c=> c.Id == trekiId).FirstOrDefaultAsync();
        if (actividad != null && treki != null)
        {            
            actividad.Treki.Add(treki);
            await context.SaveChangesAsync();
        }
    }

    public async Task RemoveTrekiFromActivity(int activityId, int trekiId)
    {
        var actividad = await context.Actividades
                        .Include(c=> c.Treki)
                        .Where(c => c.Id == activityId).FirstOrDefaultAsync();
        var treki = await context.Trekis.Where(c => c.Id == trekiId).FirstOrDefaultAsync();
        if (actividad != null && treki != null)
        {
            actividad.Treki.Remove(treki);
            await context.SaveChangesAsync();
        }
    }
}
