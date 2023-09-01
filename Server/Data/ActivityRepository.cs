using Microsoft.EntityFrameworkCore;
using SycappsWeb.Shared.Entities.Un2Trek;

namespace SycappsWeb.Server.Data;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext context;

    public ActivityRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    public async Task<Actividad> GetById(int id)
    {
        return await context.Actividades.Where(c => c.Id == id).FirstOrDefaultAsync();
    }
}
