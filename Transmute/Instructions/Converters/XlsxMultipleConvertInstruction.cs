using ClosedXML.Excel;
using System.Data;

namespace Transmute.Instructions.Converters
{
    public class XlsxMultipleConvertInstruction : BaseConvertInstruction
    {
        public XlsxMultipleConvertInstruction(string path) : base(path) { }
        public override void Execute(IEnumerable<DirectoryInfo> entities)
        {
            var resultPath = PrepareResultDirectory(Path, "xlsxResult");
            foreach (var entity in entities)
            {
                var resultFilePath = System.IO.Path.Combine(resultPath, $"{System.IO.Path.GetFileNameWithoutExtension(entity.FullName)}.xlsx");
                var dt = DeserializeEntityAsDataTable(entity);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("R1");
                    FillWorksheetHeader(dt, worksheet);

                    var rowsValues = dt.AsEnumerable()
                        .Select(row => row.ItemArray)
                        .ToList();

                    for (var row = 2; row < rowsValues.Count + 2; row++)
                        for (var column = 1; column <= dt.Columns.Count; column++)
                            worksheet.Cell(row, column).Value = XLCellValue.FromObject(rowsValues[row - 2][column - 1]);

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(resultFilePath);
                }
            }
        }
    }
}
