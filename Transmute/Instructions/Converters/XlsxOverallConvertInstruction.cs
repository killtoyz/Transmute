using ClosedXML.Excel;
using System.Data;

namespace Transmute.Instructions.Converters
{
    public class XlsxOverallConvertInstruction : BaseConvertInstruction
    {
        public XlsxOverallConvertInstruction(string path) : base(path) { }
        public override void Execute(IEnumerable<DirectoryInfo> entities)
        {
            var resultPath = PrepareResultDirectory(Path, "xlsxResult");
            var firstIteration = true;
            var resultFilePath = System.IO.Path.Combine(resultPath, "OverallFileResult.xlsx");
            var rowCount = 2;
            var columnCount = 1;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("R1");

                foreach (var entity in entities)
                {
                    var dt = DeserializeEntityAsDataTable(entity);
                    if (firstIteration)
                    {
                        FillWorksheetHeader(dt, worksheet);
                        firstIteration = false;
                    }

                    var rowsValues = dt.AsEnumerable()
                        .Select(row => row.ItemArray)
                        .ToList();

                    foreach (var row in rowsValues)
                    {
                        foreach (var value in row)
                        {
                            worksheet.Cell(rowCount, columnCount).Value = XLCellValue.FromObject(value);
                            columnCount++;
                        }

                        rowCount++;
                        columnCount = 1;
                    }
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(resultFilePath);
            }
        }
    }
}
