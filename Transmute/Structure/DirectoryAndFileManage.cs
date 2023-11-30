namespace Transmute.Structure
{
    public static class DirectoryAndFileManage
    {
        public static string GetParentDirectory(string path) =>
            Directory.GetParent(path) is null ? path : Directory.GetParent(path).FullName;

        public static IEnumerable<DirectoryInfo> GetAllDirectories(string path) =>
            Directory.EnumerateDirectories(path)
                .Select(x => new DirectoryInfo(x));

        public static IEnumerable<DirectoryInfo> GetJsonFiles(string path) =>
            Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly)
                .Select(x => new DirectoryInfo(x));
    }
}