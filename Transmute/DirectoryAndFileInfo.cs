namespace Transmute
{
    public static class DirectoryAndFileInfo
    {
        public static string GetParentDirectory(string path) =>
            Directory.GetParent(path) is null ? path : Directory.GetParent(path).Parent.FullName;

        public static IEnumerable<DirectoryInfo> GetAllDirectories(string path) =>
            Directory.EnumerateDirectories(path)
                .Select(x => new DirectoryInfo(x));

        public static IEnumerable<DirectoryInfo> GetAllFiles(string path) =>
            Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly)
                .Select(x => new DirectoryInfo(x));

        public static IEnumerable<string> GetJsonFiles(string path)
        {
            var files = new List<string>();
            var listOfJsonFiles = Directory.EnumerateFiles(path, "*.json", SearchOption.TopDirectoryOnly);
            foreach (var file in listOfJsonFiles) { files.Add(file); }
            return files;
        }
    }
}