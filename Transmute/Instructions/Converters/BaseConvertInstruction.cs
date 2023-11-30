using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Data;

namespace Transmute.Instructions.Converters
{
    public abstract class BaseConvertInstruction : IInstruction
    {
        protected BaseConvertInstruction(string path) => Path = path;
        protected readonly string Path;
        public abstract void Execute(IEnumerable<DirectoryInfo> entities);
        protected string PrepareResultDirectory(string path, string resFolder)
        {
            var newDirPath = System.IO.Path.Combine(path, resFolder);
            Directory.CreateDirectory(newDirPath);
            return newDirPath;
        }

        protected DataTable DeserializeEntityAsDataTable(DirectoryInfo jsonFile)
        {
            var jsonInput = File.ReadAllText(jsonFile.FullName);
            return (DataTable)JsonConvert.DeserializeObject(jsonInput, typeof(DataTable));
        }

        protected void FillWorksheetHeader(DataTable dt, IXLWorksheet worksheet)
        {
            for (var i = 1; i <= dt.Columns.Count; i++)
                worksheet.Cell(1, i).Value = XLCellValue.FromObject(dt.Columns[i - 1].ColumnName);
        }
    }
}
