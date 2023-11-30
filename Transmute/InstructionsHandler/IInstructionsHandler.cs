namespace Transmute.InstructionsHandler
{
    public interface IInstructionsHandler
    {
        Task Process(string path, string[] instructions);
    }
}
