namespace SycappsWeb.Client.Shared.ImportW;

public class ImportExceptionEventArgs
{
    public int RowIndex { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
