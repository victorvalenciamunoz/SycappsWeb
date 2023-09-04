using Microsoft.EntityFrameworkCore;
using SycappsWeb.Shared.Entities.Un2Trek;

namespace SycappsWeb.Server.Data;

public class TrekiRepository : ITrekiRepository
{
    private readonly ApplicationDbContext context;

    public TrekiRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    public Task Add(Treki newTreki)
    {
        context.Add(newTreki);
        return context.SaveChangesAsync();
    }

    public async Task<Treki?> GetById(int id)
    {
        var treki = await context.Trekis.Where(c => c.Id == id)
                                       .FirstOrDefaultAsync();

        return treki;
    }

    public async Task<bool> Modify(Treki trekiToModify)
    {
        var treki = await GetById(trekiToModify.Id);   
        if (treki!=null)
        {
            treki.Titulo = trekiToModify.Titulo;
            treki.Longitud = trekiToModify.Longitud;
            treki.Descripcion = trekiToModify.Descripcion;
            treki.Activo = trekiToModify.Activo;
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
    public async Task<List<Treki>> GetTrekisAround(double currentLatitude, double currentLongitude, string threshold)
    {
        return await context.Trekis.FromSqlRaw($"GetTrekisAround {currentLatitude.ToString().Replace(",", ".")}, {currentLongitude.ToString().Replace(",", ".")}, {threshold}").ToListAsync();
    }

    public async Task<List<Treki>> GetTrekiListNotInActivity(int activityId)
    {
        return await context.Trekis.FromSqlRaw($"GetTrekiListNotInActivity {activityId}").ToListAsync();
    }

    public async Task<List<Treki>> GetTrekiListByActivity(int activityId)
    {
        return await context.Trekis.FromSqlRaw($"GetTrekiListInActivity {activityId}").ToListAsync();
    }
    
    public async Task<Treki> GetTrekiByCoordinates(double latitude, double longitude)
    {
        return await context.Trekis.Where(c => c.Latitud == latitude && c.Longitud == latitude).FirstOrDefaultAsync();
    }
    
    public async Task CaptureTreki(Treki treki, string userId, int activityId)
    {
        CapturaTreki captura = new CapturaTreki();
        captura.TrekiId = treki!.Id;
        captura.UsuarioId = userId;
        captura.ActividadId = activityId;
        captura.FechaCaptura = System.DateTime.UtcNow;

        context.CapturaTrekis.Add(captura);

        await context.SaveChangesAsync();
    }

    public bool IsTrekiAlreadyCaptured(int trekiId, string userId, int activityId)
    {
        return context.CapturaTrekis.Any(c => c.UsuarioId.Equals(userId)
                                            && c.TrekiId == trekiId
                                            && c.ActividadId == activityId);
    }
}
