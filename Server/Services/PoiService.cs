using Microsoft.EntityFrameworkCore;
using SycappsWeb.Server.Data;
using SycappsWeb.Shared.Entities;
using SycappsWeb.Shared.Models;

namespace SycappsWeb.Server.Services;

public class PoiService : IPoiService
{
    private readonly ApplicationDbContext context;
    private readonly IConfiguration configuration;

    public PoiService(ApplicationDbContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    public async Task<List<PuntodeInteres>> Prueba()
    {
        var result = await context.PuntosdeInteres.Take(1).OrderBy(c => c.Id).ToListAsync();

        return result;
    }

    public async Task<List<PuntoDto>> Get(double latitude, double longitude)
    {
        var result = new List<PuntoDto>();
        var puntos = await context.PuntosdeInteres.ToListAsync();
        Geolocation.Coordinate origin = new Geolocation.Coordinate(latitude, longitude);
        foreach (var punto in puntos)
        {
            Geolocation.Coordinate destino = new Geolocation.Coordinate(punto.Latitud, punto.Longitud);
            double distance = Geolocation.GeoCalculator.GetDistance(origin, destino, decimalPlaces: 2, Geolocation.DistanceUnit.Kilometers);
            var puntoDto = (PuntoDto)(punto);
            puntoDto.DistanciaAPosicionActual = distance;
            result.Add(puntoDto);
        }
        //TODO: Esto hay que paginarlo
        int rango = Convert.ToInt32(configuration["rangoDistancia"]);
        return result.Where(c => c.DistanciaAPosicionActual <= rango).OrderBy(c => c.DistanciaAPosicionActual).Take(10).ToList();
    }

    public async Task Import(List<PuntodeInteres> puntos)
    {
        foreach (PuntodeInteres punto in puntos)
        {
            var toRemove = await context.PuntosdeInteres.Where(c => c.Latitud == punto.Latitud && c.Longitud == punto.Longitud).ToListAsync();
            if (toRemove != null && toRemove.Any())
                context.PuntosdeInteres.RemoveRange(toRemove);
        }
        //await context.SaveChangesAsync();

        context.AddRange(puntos);
        await context.SaveChangesAsync();
    }
}
