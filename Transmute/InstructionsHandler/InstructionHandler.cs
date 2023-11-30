using Transmute.Instructions;
using Transmute.Instructions.Converters;
using Transmute.Structure;

namespace Transmute.InstructionsHandler
{
    public class InstructionHandler : IInstructionsHandler
    {
        public async Task Process(string path, string[] instructions)
        {
            IEnumerable<DirectoryInfo> entities = new List<DirectoryInfo>();
            IInstruction instruction;

            foreach (var inst in instructions)
            {
                switch (inst)
                {
                    case "-a":
                        entities = await Task.Run(() => DirectoryAndFileManage.GetAllDirectories(path));
                        break;
                    case "-j":
                        entities = await Task.Run(() => DirectoryAndFileManage.GetJsonFiles(path));
                        break;
                    case "-p":
                        await Task.Run(() => { instruction = new PrintInstruction(); instruction.Execute(entities); });
                        break;
                    case "-ctm":
                        Task.Run(() => { instruction = new TxtMultipleConvertInstruction(path); instruction.Execute(entities); }).ConfigureAwait(false);
                        break;
                    case "-cjm":
                        Task.Run(() => { instruction = new XlsxMultipleConvertInstruction(path); instruction.Execute(entities); }).ConfigureAwait(false);
                        break;
                    case "-cjo":
                        Task.Run(() => { instruction = new XlsxOverallConvertInstruction(path); instruction.Execute(entities); }).ConfigureAwait(false);
                        break;
                }
            }
        }
    }
}