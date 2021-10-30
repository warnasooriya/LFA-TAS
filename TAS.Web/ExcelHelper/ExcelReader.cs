using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class ExcelReader
{
    public static IList<T> GetDataToList<T>(string filePath, Func<IList<string>, IList<string>, T> addProductData)
    {
        return GetDataToList<T>(filePath, "", addProductData);
    }

    public static IList<T> GetDataToList<T>(string filePath, string sheetName, Func<IList<string>, IList<string>, T> addProductData)
    {
        List<T> resultList = new List<T>();
        using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
        {
            WorkbookPart wbPart = document.WorkbookPart;
            Sheet sheet = null;
            WorksheetPart wsPart = null;

            if (sheetName == "")
                sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
            else
                sheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
            if (sheet != null)
                wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
            if (wsPart == null)
                throw new Exception("No worksheet.");
            else
            {
                var columnNames = new List<string>();
                var columnLetters = new List<string>();
                foreach (Cell cell in wsPart.Worksheet.Descendants<Row>().ElementAt(2))
                {
                    columnNames.Add(Regex.Replace(GetCellValue(document, cell), @"[^A-Za-z0-9_]", "").ToLower());
                    //columnNames.Add(Regex.Replace(GetCellValue(document, cell), @"[^A-Za-z0-9_ ]", ""));
                    columnLetters.Add(GetColumnAddress(cell.CellReference));
                }
                var rowData = new List<string>();
                string cellLetter = string.Empty;
                foreach (var row in GetUsedRows(document, wsPart))
                {
                    rowData.Clear();
                    foreach (var cell in GetCellsForRow(row, columnLetters))
                        rowData.Add(GetCellValue(document, cell));
                    resultList.Add(addProductData(rowData, columnNames));
                }
            }
        }
        if(resultList.Count > 0) resultList.RemoveAt(0);
        if (resultList.Count > 0) resultList.RemoveAt(0);

        return resultList;
    }

    private static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        if (cell == null) return null;
        string value = cell.InnerText;
        var x = cell.CellValue;
        if (cell.DataType != null)
        {
            switch (cell.DataType.Value)
            {
                case CellValues.SharedString:
                    var sstPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    value = sstPart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                    break;
                case CellValues.Boolean:
                    var booleanToBit = ConfigurationManager.AppSettings["BooleanToBit"];
                    if (booleanToBit != "Y")
                        value = value == "0" ? "FALSE" : "TRUE";
                    break;
            }
        }
        return value;
    }

    private static IEnumerable<Row> GetUsedRows(SpreadsheetDocument document, WorksheetPart wsPart)
    {
        bool hasValue;
        foreach (var row in wsPart.Worksheet.Descendants<Row>().Skip(1))
        {
            hasValue = false;
            foreach (var cell in row.Descendants<Cell>())
            {
                if (!string.IsNullOrEmpty(GetCellValue(document, cell)))
                {
                    hasValue = true;
                    break;
                }
            }
            if (hasValue)
                yield return row;
        }
    }

    private static IEnumerable<Cell> GetCellsForRow(Row row, List<string> columnLetters)
    {
        int workIdx = 0;
        foreach (var cell in row.Descendants<Cell>())
        {
            var cellLetter = GetColumnAddress(cell.CellReference);
            int currentActualIdx = columnLetters.IndexOf(cellLetter);
            for (; workIdx < currentActualIdx; workIdx++)
            {
                var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                yield return emptyCell;
            }
            yield return cell;
            workIdx++;
            if (cell == row.LastChild)
            {
                for (; workIdx < columnLetters.Count(); workIdx++)
                {
                    var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptyCell;
                }
            }
        }
    }

    private static string GetColumnAddress(string cellReference)
    {
        Regex regex = new Regex("[A-Za-z]+");
        Match match = regex.Match(cellReference);
        return match.Value;
    }

    public static string CheckPath(string filePath)
    {
        var checkedPath = filePath;
        if (!(checkedPath.StartsWith(@"\\") && checkedPath.Contains(@":\")))
        {
            checkedPath = GetSearchedPath(AppDomain.CurrentDomain.BaseDirectory + filePath);
            if (string.IsNullOrEmpty(checkedPath))
            {
                checkedPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf(@"\bin\") + 5) + filePath;
                checkedPath = GetSearchedPath(checkedPath);
                if (string.IsNullOrEmpty(checkedPath))
                {
                    checkedPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin\") + 1) + filePath;
                    checkedPath = GetSearchedPath(checkedPath);
                }
            }
        }
        else
            checkedPath = GetSearchedPath(checkedPath);

        if (!string.IsNullOrEmpty(checkedPath))
            return checkedPath;
        else
            throw new FileNotFoundException("Source file not found.");
    }

    private static string GetSearchedPath(string searchPath)
    {
        var pos = searchPath.LastIndexOf(@"\") + 1;
        var pod = searchPath.LastIndexOf(@".");
        var srcfolder = searchPath.Substring(0, pos);
        var filePureName = searchPath.Substring(pos, pod - pos);
        var fileDotExt = searchPath.Substring(pod);

        DirectoryInfo dir = new DirectoryInfo(srcfolder);
        if (!dir.Exists) return null;
        var srcFirstFile = dir.GetFiles().Where(fi => fi.Name.ToLower().Contains(filePureName.ToLower())
                  && fi.Name.ToLower().Contains(fileDotExt)).OrderByDescending(fi => fi.LastWriteTime).FirstOrDefault();
        if (srcFirstFile != null && srcFirstFile.Exists)
            return srcFirstFile.FullName;
        return null;
    }

    public static int IndexFor(this IList<string> list, string name)
    {
        int idx = list.IndexOf(Regex.Replace(name.Replace(" ", ""), @"[^A-Za-z0-9_]", "").ToLower());
        if (idx < 0)
            throw new Exception(string.Format("Missing required column mapped to: {0}.", name));
        return idx;
    }

    #region String Converters
    public static int ToInt32(this string source)
    {
        int outNum;
        return int.TryParse(source, out outNum) ? outNum : 0;
    }
    public static int? ToInt32Nullable(this string source)
    {
        int outNum;
        return int.TryParse(source, out outNum) ? outNum : (int?)null;
    }
    public static decimal ToDecimal(this string source)
    {
        decimal outNum;
        return decimal.TryParse(source, out outNum) ? outNum : 0;
    }

    public static decimal? ToDecimalNullable(this string source)
    {
        decimal outNum;
        return decimal.TryParse(source, out outNum) ? outNum : (decimal?)null;
    }

    public static double ToDouble(this string source)
    {
        double outNum;
        return double.TryParse(source, out outNum) ? outNum : 0;
    }

    public static double? ToDoubleNullable(this string source)
    {
        double outNum;
        return double.TryParse(source, out outNum) ? outNum : (double?)null;
    }

    public static DateTime ToDateTime(this string source)
    {
        DateTime outDt;
        if (DateTime.TryParse(source, out outDt))
        {
            return outDt;
        }
        else
        {
            if (IsNumeric(source))
            {
                return DateTime.FromOADate(source.ToDouble());
            }
            return DateTime.Now;
        }
    }

    public static DateTime? ToDateTimeNullable(this string source)
    {
        DateTime outDt;
        if (DateTime.TryParse(source, out outDt))
        {
            return outDt;
        }
        else
        {
            if (IsNumeric(source))
            {
                return DateTime.FromOADate(source.ToDouble());
            }
            return (DateTime?)null;
        }
    }

    public static bool ToBoolean(this string source)
    {
        if (!string.IsNullOrEmpty(source))
            if (source.ToLower() == "true" || source == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        else
        {
            return false;
        }
    }
    public static bool? ToBooleanNullable(this string source)
    {
        if (!string.IsNullOrEmpty(source))
            if (source.ToLower() == "true" || source == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        else
        {
            return (bool?)null;
        }
    }

    public static Guid ToGuid(this string source)
    {
        Guid outGuid;
        return Guid.TryParse(source, out outGuid) ? outGuid : Guid.Empty;
    }

    public static Guid? ToGuidNullable(this string source)
    {
        Guid outGuid;
        return Guid.TryParse(source, out outGuid) ? outGuid : (Guid?)null;
    }
    #endregion

    #region Util
    private static readonly Regex _isNumericRegex = new Regex("^(" +
        /*Hex*/ @"0x[0-9a-f]+" + "|" +
        /*Bin*/ @"0b[01]+" + "|" +
        /*Oct*/ @"0[0-7]*" + "|" +
        /*Dec*/ @"((?!0)|[-+]|(?=0+\.))(\d*\.)?\d+(e\d+)?" +
        ")$");
    static bool IsNumeric(string value)
    {
        return _isNumericRegex.IsMatch(value);
    }
    #endregion
}

