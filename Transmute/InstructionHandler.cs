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
    }
}