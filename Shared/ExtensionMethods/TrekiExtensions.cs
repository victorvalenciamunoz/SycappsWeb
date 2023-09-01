using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Shared.ExtensionMethods;

public static class TrekiExtensions
{
    public static Treki ToEntity(this TrekiResponse treki)
    {
        return new Treki
        {
            Descripcion = treki.Description,
            Latitud = treki.Latitude,
            Longitud = treki.Longitude,
            Titulo = treki.Title
        };
    }

    public static TrekiResponse ToResponse(this Treki treki)
    {
        return new TrekiResponse
        {
            Description = treki.Descripcion,
            Latitude = treki.Latitud,
            Longitude = treki.Longitud,
            Title = treki.Titulo
        };
    }
}
