using Syncfusion.XlsIO;
using System.Data;

namespace SycappsWeb.Client.Shared.ImportW;

public class ExcelImport
{
    public DataTable LoadData(MemoryStream inputStream, int workSheetIndex = 0, int recordsToReturn = 0)
    {
        List<string> columns = new List<string>();
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            IApplication application = excelEngine.Excel;

            //Set the default application version
            application.DefaultVersion = ExcelVersion.Xlsx;

            inputStream.Position = 0;
            IWorkbook workbook = application.Workbooks.Open(inputStream);

            IWorksheet worksheet = workbook.Worksheets[workSheetIndex];
            var table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
            var row = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                row[column.Ordinal] = column.ColumnName;
            }
            table.Rows.InsertAt(row, 0);
            if (recordsToReturn > 0 && table.Rows.Count >= recordsToReturn)
                return table.AsEnumerable().Take(recordsToReturn).CopyToDataTable();
            else
                return table;

        }
    }

    public void Import(FileStream inputStream, int workSheetIndex)
    {
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            IApplication application = excelEngine.Excel;

            //Set the default application version
            application.DefaultVersion = ExcelVersion.Xlsx;

            IWorkbook workbook = application.Workbooks.Open(inputStream);

            IWorksheet worksheet = workbook.Worksheets[workSheetIndex];

            var table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);

        }
    }
}
