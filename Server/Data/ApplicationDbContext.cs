using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Entities.Un2Trek;

namespace SycappsWeb.Server.Data;

public class ApplicationDbContext: IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Treki> Trekis { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<CapturaTreki> CapturaTrekis { get; set; }

    public DbSet<PuntodeInteres> PuntosdeInteres { get; set; }
    public DbSet<MensajeContacto> MensajesContacto { get; set; }
}
