using Transmute.Structure;

namespace Transmute
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userInputParser = new UserInputStringParser();
            var consoleController = new ConsoleController(userInputParser);
            consoleController.MainCycle();
        }
    }
}