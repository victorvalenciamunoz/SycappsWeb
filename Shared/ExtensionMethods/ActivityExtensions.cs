using SycappsWeb.Shared.Entities.Un2Trek;
using SycappsWeb.Shared.Models.Un2Trek;

namespace SycappsWeb.Shared.ExtensionMethods;

public static class ActivityExtensions
{
    public static ActividadRequest ToRequest(this ActividadResponse response)
    {
        DateTime? validoHasta = null;
        if (!string.IsNullOrEmpty(response.ValidoHasta))
        {
            validoHasta = Convert.ToDateTime(response.ValidoHasta);
        }
        return new ActividadRequest(response.Titulo, Convert.ToDateTime(response.ValidoDesde), validoHasta);
    }
    
    public static ActividadResponse ToResponse(this Actividad actividad)
    {
        string validoHasta = string.Empty;

        if (actividad.ValidoHasta.HasValue)
        {
            validoHasta = actividad.ValidoHasta!.ToString();
        }

        return new ActividadResponse(actividad.Id, actividad.Titulo, actividad.ValidoDesde.ToString(), validoHasta);
    }
    
    public static Actividad ToEntity(this ActividadRequest request)
    {
        return new Actividad
        {
            Titulo = request.Titulo,
            ValidoDesde = request.ValidoDesde,
            ValidoHasta = request.ValidoHasta
        };
    }
}

