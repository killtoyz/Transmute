using Transmute.Instructions;
using Transmute.Instructions.Converters;
using Transmute.Structure;

namespace Transmute.InstructionsHandler
{
    public class InstructionHandler : IInstructionsHandler
    {
        public void Process(string path, string[] instructions)
        {
            IEnumerable<DirectoryInfo> entities = new List<DirectoryInfo>();
            IInstruction instruction;

            foreach (var inst in instructions)
            {
                switch (inst)
                {
                    case "-a":
                        entities = DirectoryAndFileManage.GetAllDirectories(path);
                        break;
                    case "-j":
                        entities = DirectoryAndFileManage.GetJsonFiles(path);
                        break;
                    case "-p":
                        instruction = new PrintInstruction();
                        instruction.Execute(entities);
                        break;
                    case "-ctm":
                        instruction = new TxtMultipleConvertInstruction(path);
                        instruction.Execute(entities);
                        break;
                    case "-cjm":
                        instruction = new XlsxMultipleConvertInstruction(path);
                        instruction.Execute(entities);
                        break;
                    case "-cjo":
                        instruction = new XlsxOverallConvertInstruction(path);
                        instruction.Execute(entities);
                        break;
                }
            }
        }
    }
}