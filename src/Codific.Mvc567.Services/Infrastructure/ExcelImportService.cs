using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Codific.Mvc567.Services.Abstractions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class ExcelImportService : IExcelImportService
    {
        public List<string[]> ExtractLinesOfExcelFile(string path, int rows, int columns, int sheetIndex = 0)
        {
            var resultList = new List<string[]>();
            try
            {
                ISheet sheet = this.ExtractSheetFromPath(path, sheetIndex);

                for (int i = 0; i < rows; i++)
                {
                    string[] currentRow = new string[columns];
                    IRow row = sheet.GetRow(i);
                    for (int j = 0; j < columns; j++)
                    {
                        ICell cell = row.GetCell(j);
                        currentRow[j] = cell?.ToString();
                    }

                    var currentRowToString = string.Join(" ", currentRow);
                    if (!string.IsNullOrWhiteSpace(currentRowToString))
                    {
                        resultList.Add(currentRow);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return resultList;
        }

        public IEnumerable<TModel> MapContentToModel<TModel>(List<string[]> content, Dictionary<string, int> schemeDictionary, bool hasHeader)
        {
            List<TModel> resultList = new List<TModel>();
            Type modelType = typeof(TModel);

            if (hasHeader)
            {
                content.RemoveAt(0);
            }

            for (int i = 0; i < content.Count; i++)
            {
                try
                {
                    TModel currentModel = Activator.CreateInstance<TModel>();
                    string[] contentRow = content[i];
                    foreach (var schemeItem in schemeDictionary)
                    {
                        try
                        {
                            PropertyInfo schemeItemProperty = modelType.GetProperty(schemeItem.Key);
                            Type schemeItemType = schemeItemProperty.PropertyType;
                            object schemeItemValue = Convert.ChangeType(contentRow[schemeItem.Value], schemeItemType);
                            schemeItemProperty.SetValue(currentModel, schemeItemValue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }

                    resultList.Add(currentModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return resultList;
        }

        private ISheet ExtractSheetFromPath(string path, int sheetIndex = 0)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                ISheet sheet;
                if (path.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                    sheet = hssfwb.GetSheetAt(sheetIndex);
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                    sheet = hssfwb.GetSheetAt(sheetIndex);
                }

                return sheet;
            }
        }
    }
}
