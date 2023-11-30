namespace Transmute.InstructionsHandler
{
    public interface IInstructionsHandler
    {
        void Process(string path, string[] instructions);
    }
}
