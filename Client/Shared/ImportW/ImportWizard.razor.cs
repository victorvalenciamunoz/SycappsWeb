using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using System.Data;
using System.Reflection;

namespace SycappsWeb.Client.Shared.ImportW;

public partial class ImportWizard<TValue> where TValue : new()
{
    private Dictionary<string, string> entityProperties = new Dictionary<string, string>();

    private DataTable previewData = new DataTable();

    private bool firstRowHeaders;

    private int columnIndexSelected;

    private Dictionary<int, string> mappings = new Dictionary<int, string>();

    private string selectedProperty = string.Empty;

    private MemoryStream? streamToImport = null;


    [Parameter]
    public EventCallback ImportCompleted { get; set; }

    [Parameter]
    public EventCallback<ImportExceptionEventArgs> ExceptionRaised { get; set; }

    protected override void OnParametersSet()
    {
        var type = typeof(TValue);
        if (type is null) return;

        var properties = type.GetProperties();
        if (properties.Length == 0) return;

        entityProperties = new Dictionary<string, string>();
        foreach (var property in properties)
        {
            if (string.IsNullOrWhiteSpace(property.Name)) continue;
            entityProperties.Add(property.Name, property.Name);
        }

        base.OnParametersSet();
    }

    private void Import()
    {
        List<TValue> importedObjects = new List<TValue>();

        ExcelImport import = new ExcelImport();

        if (streamToImport is not null)
        {
            var currentRowIndex = 1;
            PropertyInfo currentProperty = null;
            try
            {
                var importedData = import.LoadData(streamToImport);
                foreach (DataRow row in importedData.Rows)
                {
                    if (firstRowHeaders && currentRowIndex == 1)
                    {
                        currentRowIndex += 1;
                        continue;
                    }
                    TValue classToImport = new TValue();
                    if (classToImport is null) return;

                    var currentColumnIndex = 0;
                    foreach (DataColumn column in importedData.Columns)
                    {
                        if (mappings.Any(c => c.Key == currentColumnIndex))
                        {
                            currentProperty = classToImport.GetType().GetProperty(mappings[currentColumnIndex]);
                            if (row[column] is not null && row[column] is not DBNull)
                            {
                                if (currentProperty.PropertyType == typeof(double))
                                {
                                    currentProperty.SetValue(classToImport, Convert.ToDouble(row[column].ToString().Replace(".", ",")));
                                    //currentProperty.SetValue(classToImport, Convert.ToDouble(row[column]));
                                }
                                else if (currentProperty.PropertyType == typeof(int))
                                {
                                    currentProperty.SetValue(classToImport, Convert.ToInt32(row[column]));
                                }
                                else
                                {
                                    currentProperty.SetValue(classToImport, row[column]);
                                }
                            }
                        }
                        currentColumnIndex += 1;
                    }
                    currentRowIndex += 1;
                    importedObjects.Add(classToImport);
                }
                ImportCompleted.InvokeAsync(importedObjects);
            }
            catch (Exception ex)
            {
                ExceptionRaised.InvokeAsync(new ImportExceptionEventArgs
                {
                    Message = ex.Message,
                    RowIndex = currentRowIndex,
                    PropertyName = currentProperty?.Name
                });
            }
        }
    }

    private void OnChange(UploadChangeEventArgs args)
    {
        if (args != null && args.Files.Count > 0 && args.Files[0] is not null)
        {
            ExcelImport import = new ExcelImport();
            streamToImport = args.Files[0].Stream;
            previewData = import.LoadData(streamToImport, recordsToReturn: 10);
        }
    }

    private void MappingChanged(ChangeEventArgs e)
    {
        if (e is null || e.Value is null) return;

        if (mappings.Any(c => c.Key == columnIndexSelected))
        {
            mappings[columnIndexSelected] = e.Value.ToString();
        }
        else
        {
            mappings.Add(columnIndexSelected, e.Value.ToString());
        }
    }

    private void SelectedColumn(int columnIndex)
    {
        columnIndexSelected = columnIndex;
        if (mappings.Any(c => c.Key == columnIndexSelected))
        {
            selectedProperty = mappings[columnIndexSelected];
        }
        else
        {
            selectedProperty = string.Empty;
        }
        ShouldRender();
    }
}
