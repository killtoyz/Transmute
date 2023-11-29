using Newtonsoft.Json;
using System.Data;
using DataTable = System.Data.DataTable;
using ClosedXML.Excel;

namespace Transmute
{
    public class InstructionHandler
    {
        private readonly string[] _commands;
        public InstructionHandler(string[] commands)
        {
            _commands = commands;
        }

        public void ProcessInstructions(string path)
        {
            IEnumerable<DirectoryInfo> entities = new List<DirectoryInfo>();

            foreach (var command in _commands)
            {
                switch (command)
                {
                    case "-a":
                        entities = DirectoryAndFileManage.GetAllDirectories(path);
                        break;
                    case "-j":
                        entities = DirectoryAndFileManage.GetAllFiles(path);
                        break;
                    case "-p":
                        PrintInstruction(entities);
                        break;
                    case "-ctm":
                    {
                        var resultPath = PrepareResultDirectory(path, "TxtResult");
                        ConvertingJsonToTxtMultipleFiles(entities, resultPath);
                        break;
                    }
                    case "-cjm":
                    {
                        var resultPath = PrepareResultDirectory(path, "XlxsResult");
                        ConvertJsonToXlsxMultipleFiles(entities, resultPath);
                        break;
                    }
                    case "-cjo":
                    {
                        var resultPath = PrepareResultDirectory(path, "XlxsResult");
                        ConvertJsonToXlsxOneFile(entities, resultPath);
                        break;
                    }
                }
            }
        }
        private void PrintInstruction(IEnumerable<DirectoryInfo> listOfEntities)
        {
            try
            {
                foreach (var dir in listOfEntities)
                {
                    Console.Write($"{dir.Name} ");
                    ConsoleWriteExtension.Write($"{dir.Extension} ", ConsoleColor.Blue);
                    ConsoleWriteExtension.Write($"[{dir.Attributes}]", ConsoleColor.DarkYellow);
                    Console.WriteLine();
                }
            }
            catch (UnauthorizedAccessException)
            {
                ConsoleWriteExtension.WriteLine($"UnauthorizedAccessException. Please check access to this file or folder", ConsoleColor.Red);
            }

            Console.WriteLine();
        }

        private string PrepareResultDirectory(string path, string resFolder)
        {
            var newDirPath = Path.Combine(path, resFolder);
            Directory.CreateDirectory(newDirPath);
            return newDirPath;
        }

        private void ConvertingJsonToTxtMultipleFiles(IEnumerable<DirectoryInfo> entities, string resultPath)
        {
            foreach(var entity in entities)
            {
                string jsonInput = File.ReadAllText(entity.FullName);
                var resultFilePath = Path.Combine(resultPath, $"{Path.GetFileNameWithoutExtension(entity.FullName)}.txt");
                File.WriteAllText(resultFilePath, jsonInput);
            }
        }

        private void ConvertJsonToXlsxMultipleFiles(IEnumerable<DirectoryInfo> entities, string resultPath)
        {
            foreach (var entity in entities)
            {
                var resultFilePath = Path.Combine(resultPath, $"{Path.GetFileNameWithoutExtension(entity.FullName)}.xlsx");
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

        private void ConvertJsonToXlsxOneFile(IEnumerable<DirectoryInfo> entities, string resultPath)
        {
            bool firstIteration = true;
            var resultFilePath = Path.Combine(resultPath, "OneFileResult.xlsx");
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

                    foreach(var row in rowsValues)
                    {
                        foreach(var value in row)
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

        private DataTable DeserializeEntityAsDataTable(DirectoryInfo jsonFile)
        {
            var jsonInput = File.ReadAllText(jsonFile.FullName);
            return (DataTable)JsonConvert.DeserializeObject(jsonInput, typeof(DataTable));
        }

        private void FillWorksheetHeader(DataTable dt, IXLWorksheet worksheet)
        {
            for (var i = 1; i <= dt.Columns.Count; i++)
                worksheet.Cell(1, i).Value = XLCellValue.FromObject(dt.Columns[i - 1].ColumnName);
        }
    }
}