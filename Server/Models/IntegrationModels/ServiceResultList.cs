namespace SycappsWeb.Server.Models.IntegrationModels;

public class ServiceResultList<T> : ServiceResult
{
    public List<T> Elements { get; set; }
}
