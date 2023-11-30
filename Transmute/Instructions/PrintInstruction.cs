using Transmute.Structure;

namespace Transmute.Instructions
{
    public class PrintInstruction : IInstruction
    {
        public void Execute(IEnumerable<DirectoryInfo> entities)
        {
            try
            {
                foreach (var dir in entities)
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
        }
    }
}
