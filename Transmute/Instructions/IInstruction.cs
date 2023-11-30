namespace Transmute.Instructions
{
    public interface IInstruction
    {
        void Execute(IEnumerable<DirectoryInfo> entities);
    }
}
