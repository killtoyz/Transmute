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
                var pathEndingEntity = PathEndingEntity.CheckEndingEntity(_userInputStringParser.Path);

                InstructionHandler instructionHandler = new InstructionHandler(_userInputStringParser.Commands);
                instructionHandler.ProcessInstructions(_userInputStringParser.Path);
            }
        }

        private void InitMainInstruction()
        {
            ConsoleWriteExtension.Write(
                $"Enter directory or file path in next format: {_userInputLine}[d:\\JSONExamples\\test.json] or [d:\\\\JSONExamples]", ConsoleColor.DarkYellow);
            Console.WriteLine();
        }
    }
}
