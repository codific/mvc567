using System.Collections.Generic;

namespace Codific.Mvc567.Services.Abstractions
{
    public interface IExcelImportService
    {
        List<string[]> ExtractLinesOfExcelFile(string path, int rows, int columns, int sheetIndex = 0);

        int GetExcelFileRows(string path, int columnsToRead, int tolerance = 1, int sheetIndex = 0);

        IEnumerable<TModel> MapContentToModel<TModel>(List<string[]> content, Dictionary<string, int> schemeDictionary, bool hasHeader, int rowsToRead);
    }
}
