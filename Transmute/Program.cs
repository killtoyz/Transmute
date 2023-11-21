namespace Transmute
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userInputLine = $"{Environment.UserName}:> ";
            ConsoleWriteExtension.Write(
                $"Enter directory or file path in next format: {userInputLine}[d:\\JSONExamples\\test.json] or [d:\\\\JSONExamples]", ConsoleColor.DarkYellow);
            Console.WriteLine();

            var userInputParser = new UserInputStringParser();

            var path = string.Empty;

            var firstTimeEnterCycle = true;

            while (true)
            {
                if (firstTimeEnterCycle)
                {
                    ConsoleWriteExtension.Write(userInputLine, ConsoleColor.Cyan);
                    path = Console.ReadLine() + Path.DirectorySeparatorChar;
                    userInputParser.Parse(path);

                    path = userInputParser.Path;
                }
                else
                {
                    ConsoleWriteExtension.WriteLine(path, ConsoleColor.Green);
                    ConsoleWriteExtension.Write(userInputLine, ConsoleColor.Cyan);
                    
                    var continueInput = Console.ReadLine();
                    if (continueInput.StartsWith("..")) path = DirectoryAndFileManage.GetParentDirectory(path);
                    else path = Path.Combine(path, continueInput);

                    userInputParser.Parse(path);
                    path = userInputParser.Path;
                }

                var pathEndingEntity = PathEndingEntity.CheckEndingEntity(path);

                if (pathEndingEntity == PathEndingEntityEnum.Directory)
                {
                    ConsoleWriteExtension.WriteLine($"Its directory [{path}]", ConsoleColor.Green);
                    firstTimeEnterCycle = false;

                    var listOfDirectories = DirectoryAndFileManage.GetAllDirectories(path);
                    var listOfJsonFiles = DirectoryAndFileManage.GetAllFiles(path);

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
                        ConsoleWriteExtension.WriteLine($"UnauthorizedAccessException [{path}]. Please check access", ConsoleColor.Red);
                    }

                    Console.WriteLine();
                }
                else if (pathEndingEntity == PathEndingEntityEnum.File)
                {
                    ConsoleWriteExtension.WriteLine($"Its file [{path}]", ConsoleColor.Green);
                }
                else
                {
                    ConsoleWriteExtension.WriteLine($"Directory or file does not exists [{path}]", ConsoleColor.Red);
                    userInputParser.Clear();
                    path = userInputParser.Path;
                    firstTimeEnterCycle = false;
                }
            }
        }
    }
}