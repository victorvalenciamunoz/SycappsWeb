using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SycappsWeb.Shared.Entities;


namespace SycappsWeb.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<PuntodeInteres> PuntosdeInteres { get; set; }
    public DbSet<MensajeContacto> MensajesContacto { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuraciones adicionales del modelo

        builder.Entity<PuntodeInteres>()
            .Property<byte[]>("RowVersion")
            .IsRowVersion();
    }
}
