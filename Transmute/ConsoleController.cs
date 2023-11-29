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
                ConsoleWriteExtension.WriteLine(_userInputStringParser.Path, ConsoleColor.Green);
                ConsoleWriteExtension.Write(_userInputLine, ConsoleColor.Cyan);

                var line = Console.ReadLine();
                _userInputStringParser.Parse(line);
                
                if (VerifyPath(_userInputStringParser.Path))
                {
                    ConsoleWriteExtension.WriteLine($"Path does not exist [{_userInputStringParser.Path}]", ConsoleColor.Red);
                    _userInputStringParser.Clear();
                    continue;
                }

                var instructionHandler = new InstructionHandler(_userInputStringParser.Commands);
                instructionHandler.ProcessInstructions(_userInputStringParser.Path);
            }
        }

        private void InitMainInstruction()
        {
            ConsoleWriteExtension.Write(
                $"Enter directory or file path in next format: {_userInputLine}[d:\\JSONExamples\\test.json] or [d:\\\\JSONExamples] " +
                $"\nor just any folder name if you are inside disk already" + 
                "\nCommands: \n'-a' - get all directories \n'-j' - get only JSON files \n'-p' - print all files and dirs " +
                "\n'-ctm' - convert all files to txt files \n'-cjm' - convert all json files to xlsx one by one \n'-cjo' - convert all json files to one xlsx table" +
                "\nFull input can be like this chain - [d:] then [JSONExamples -j -p]" +
                "\nTo convert JSON files you need to input - [directory path] [-j](getting json files collection)[-cjo]", ConsoleColor.DarkYellow);
            Console.WriteLine();
        }

        private bool VerifyPath(string path) => PathEndingEntity.CheckEndingEntity(path) == PathEndingEntityEnum.NotExist;
    }
}
