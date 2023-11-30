namespace Transmute.Instructions.Converters
{
    public class TxtMultipleConvertInstruction : BaseConvertInstruction
    {
        public TxtMultipleConvertInstruction(string path) : base(path) { }
        public override void Execute(IEnumerable<DirectoryInfo> entities)
        {
            var resultPath = PrepareResultDirectory(Path, "txtResult");
            foreach (var entity in entities)
            {
                string jsonInput = File.ReadAllText(entity.FullName);
                var resultFilePath = System.IO.Path.Combine(resultPath, $"{System.IO.Path.GetFileNameWithoutExtension(entity.FullName)}.txt");
                File.WriteAllText(resultFilePath, jsonInput);
            }
        }
    }
}
