namespace Transmute
{
    public class UserInputStringParser
    {
        private static char[] separators = { ' ' };
        public string? UserPreviousCommand { get; private set; }

        private string? _path;
        public string Path
        {
            get
            {
                if (_path == null) { return ""; }
                if (!_path.EndsWith("\\")) return _path + System.IO.Path.DirectorySeparatorChar;
                return _path;
            }

            private set
            {
                if (value == "..")
                    _path = DirectoryAndFileManage.GetParentDirectory(_path);
                else if (_path != null)
                    _path = System.IO.Path.Combine(_path, value);
                else
                    _path = value;
            }
        }
        public string[]? Commands { get; private set; }
        public void Parse(string input)
        {
            var parts = input.Split(separators).ToList();
            UserPreviousCommand = string.Join(' ', parts);

            Path = parts[0];
            parts.RemoveAt(0);

            Commands = parts.ToArray();
        }

        public void Clear()
        {
            _path = string.Empty;
            Commands = null;
        }
    }
}