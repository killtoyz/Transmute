namespace Transmute
{
    public class ConsoleController
    {
        public ConsoleController(UserInputStringParser userInputStringParser)
        {
            this._userInputStringParser = userInputStringParser;
        }

        private readonly UserInputStringParser _userInputStringParser;
        private readonly string _userInputLine = Environment.UserName + ":> ";

        public void MainCycle()
        {
            InitMainInstruction();

            while (true)
            {
                var instructions = GetInstructions();
                var pathEndingEntity = PathEndingEntity.CheckEndingEntity(instructions);
                SolveInstructions(pathEndingEntity, instructions);
            }
        }

        private void InitMainInstruction()
        {
            ConsoleWriteExtension.Write(
                $"Enter directory or file path in next format: {_userInputLine}[d:\\JSONExamples\\test.json] or [d:\\\\JSONExamples]", ConsoleColor.DarkYellow);
            Console.WriteLine();
        }

        private string GetInstructions()
        {
            var path = _userInputStringParser.Path;
            ConsoleWriteExtension.WriteLine(path, ConsoleColor.Green);
            ConsoleWriteExtension.Write(_userInputLine, ConsoleColor.Cyan);

            var continueInput = Console.ReadLine();
            if (continueInput.StartsWith("..")) path = DirectoryAndFileManage.GetParentDirectory(path);
            else path = Path.Combine(path, continueInput);

            _userInputStringParser.Parse(path);
            path = _userInputStringParser.Path;

            return path;
        }

        private void SolveInstructions(PathEndingEntityEnum pathEndingEntity, string instructions)
        {
            if (pathEndingEntity == PathEndingEntityEnum.Directory)
            {
                ConsoleWriteExtension.WriteLine($"Its directory [{instructions}]", ConsoleColor.Green);

                var listOfDirectories = DirectoryAndFileManage.GetAllDirectories(instructions);
                var listOfJsonFiles = DirectoryAndFileManage.GetAllFiles(instructions);

                try
                {
                    foreach (var dir in listOfDirectories)
                    {
                        Console.Write($"{dir.Name} ");
                        ConsoleWriteExtension.Write($"{dir.Extension} ", ConsoleColor.Blue);
                        ConsoleWriteExtension.Write($"[{dir.Attributes}]", ConsoleColor.DarkYellow);
                        Console.WriteLine();
                    }

                    foreach (var file in listOfJsonFiles)
                    {
                        Console.Write($"{file.Name} ");
                        ConsoleWriteExtension.Write($"{file.Extension} ", ConsoleColor.Blue);
                        Console.WriteLine();
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    ConsoleWriteExtension.WriteLine($"UnauthorizedAccessException [{instructions}]. Please check access", ConsoleColor.Red);
                }

                Console.WriteLine();
            }
            else if (pathEndingEntity == PathEndingEntityEnum.File)
            {
                ConsoleWriteExtension.WriteLine($"Its file [{instructions}]", ConsoleColor.Green);
            }
            else
            {
                ConsoleWriteExtension.WriteLine($"Directory or file does not exists [{instructions}]", ConsoleColor.Red);
                _userInputStringParser.Clear();
                instructions = _userInputStringParser.Path;
            }
        }
    }
}
