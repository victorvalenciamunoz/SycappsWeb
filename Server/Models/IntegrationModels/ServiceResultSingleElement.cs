namespace SycappsWeb.Server.Models.IntegrationModels;

public class ServiceResultSingleElement<T> : ServiceResult
{
    public T Element { get; set; }
}
