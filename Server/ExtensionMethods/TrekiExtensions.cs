using SycappsWeb.Server.Models.Un2Trek;
using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Server.ExtensionMethods;

public static class TrekiExtensions
{
    public static Treki ToEntity(this TrekiDto treki)
    {
        return new Treki
        {
            Descripcion = treki.Descripcion,
            Latitud = treki.Latitud,
            Longitud = treki.Longitud,
            Titulo = treki.Titulo,
            Id = treki.Id,
            Activo = treki.Activo
        };
    }
    public static TrekiResponse ToResponse(this TrekiDto treki)
    {
        return new TrekiResponse
        {
            Id = treki.Id,
            Description = treki.Descripcion,
            Latitude = treki.Latitud,
            Longitude = treki.Longitud,
            Title = treki.Titulo
        };
    }
    public static TrekiDto ToDto(this TrekiRequest treki)
    {
        return new TrekiDto
        {
            Id = treki.Id,
            Descripcion = treki.Description,
            Latitud = treki.Latitude,
            Longitud = treki.Longitude,
            Titulo = treki.Title
        };
    }

    public static TrekiDto ToDto(this Treki treki)
    {
        return new TrekiDto
        {
            Id = treki.Id,
            Descripcion = treki.Descripcion,
            Latitud = treki.Latitud,
            Longitud = treki.Longitud,
            Titulo = treki.Titulo
        };
    }
}
