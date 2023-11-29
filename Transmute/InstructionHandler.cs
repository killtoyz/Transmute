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
                    case "-c":
                    {
                        var resultPath = PrepareResultDirectory(path);
                        ConvertingJsonAsTxt(entities, resultPath);
                        ConvertingJsonAsXlsx(entities, resultPath);
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

        private string PrepareResultDirectory(string path)
        {
            var newDirPath = Path.Combine(path, "Result");
            Directory.CreateDirectory(newDirPath);
            return newDirPath;
        }

        private void ConvertingJsonAsTxt(IEnumerable<DirectoryInfo> entities, string resultPath)
        {
            foreach(var entity in entities)
            {
                string jsonInput = File.ReadAllText(entity.FullName);
                var resultFilePath = Path.Combine(resultPath, $"{Path.GetFileNameWithoutExtension(entity.FullName)}.txt");
                File.WriteAllText(resultFilePath, jsonInput);
            }
        }

        private void ConvertingJsonAsXlsx(IEnumerable<DirectoryInfo> entities, string resultPath)
        {
            foreach(var entity in entities)
            {
                var jsonInput = File.ReadAllText(entity.FullName);
                var resultFilePath = Path.Combine(resultPath, $"{Path.GetFileNameWithoutExtension(entity.FullName)}.xlsx");

                var dt = (DataTable)JsonConvert.DeserializeObject(jsonInput, (typeof(DataTable)));

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("R1");

                    for(var i = 1; i <= dt.Columns.Count; i++)
                        worksheet.Cell(1,i).Value = XLCellValue.FromObject(dt.Columns[i - 1].ColumnName);

                    var rowsValues = dt.AsEnumerable()
                        .Select(row => row.ItemArray)
                        .ToList();

                    for (var row = 2; row < rowsValues.Count + 2; row++)
                        for (var column = 1; column <= dt.Columns.Count; column++)
                            worksheet.Cell(row, column).Value = XLCellValue.FromObject(rowsValues[row - 2][column - 1]);

                    workbook.SaveAs(resultFilePath);
                }
            }
        }
    }
}