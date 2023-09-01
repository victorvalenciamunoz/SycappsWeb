namespace SycappsWeb.Server.Models.IntegrationModels;

public class ServiceResult
{
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
    public List<string> Messages { get; set; }
    //
    // Resumen:
    //     True si hay Errores en el Result
    public bool HasErrors => Errors != null && Errors.Any();
    //
    // Resumen:
    //     True si hay Mensajes en el Result
    public bool HasMessages => Messages != null && Messages.Any();
    //
    // Resumen:
    //     True si hay Advertencias en el Result
    public bool HasWarnings => Warnings != null && Warnings.Any();

    public ServiceResult()
    {
        Errors = new List<string>();
    }
}
