namespace Transmute
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine();

            UserInputStringParser userInputParser = new UserInputStringParser();

            var userInputLine = $"{Environment.UserName}:> ";
            var path = string.Empty;

            var firstTimeEnterCycle = true;

            while (true)
            {
                if (firstTimeEnterCycle)
                {
                    path = Console.ReadLine() + Path.DirectorySeparatorChar;
                    userInputParser.Parse(path);

                    path = userInputParser.Path;
                }
                else
                {
                    var continueInput = Console.ReadLine();

                    if (continueInput.StartsWith("..")) path = DirectoryAndFileInfo.GetParentDirectory(path);
                    else path = Path.Combine(path, continueInput);

                    userInputParser.Parse(path);
                    path = userInputParser.Path;
                }

                var pathEndingEntity = PathEndingEntity.CheckEndingEntity(path);

                if (pathEndingEntity == PathEndingEntityEnum.Directory)
                {
                    firstTimeEnterCycle = false;

                    var listOfDirectories = DirectoryAndFileInfo.GetAllDirectories(path);
                    var listOfJsonFiles = DirectoryAndFileInfo.GetAllFiles(path);

                    try
                    {
                        foreach (var dir in listOfDirectories)
                        {
                            Console.Write($"{dir.Name} ");
                            Console.WriteLine();
                        }

                        foreach (var file in listOfJsonFiles)
                        {
                            Console.Write($"{file.Name} ");
                            Console.WriteLine();
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {

                    }

                    Console.WriteLine();
                }
                else if (pathEndingEntity == PathEndingEntityEnum.File)
                {

                }
                else
                {
                    userInputParser.Clear();
                    path = userInputParser.Path;
                    firstTimeEnterCycle = false;
                }
            }
        }
    }
}
