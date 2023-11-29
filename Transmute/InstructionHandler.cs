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
                if (command == "-a")
                {
                    entities = DirectoryAndFileManage.GetAllDirectories(path);
                }
                else if (command == "-j")
                {
                    entities = DirectoryAndFileManage.GetAllFiles(path);
                }
                else if (command == "-p")
                {
                    PrintInstruction(entities);
                }
                else if (command == "-c")
                {
                    var resultPath = PrepareResultDirectory(path);
                    ConvertingJsonAsTxt(entities, resultPath);
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
    }
}