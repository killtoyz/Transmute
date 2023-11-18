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
                if (!_path.EndsWith("\\")) return _path + System.IO.Path.DirectorySeparatorChar;
                return _path;
            }
        }
        public string[]? Commands { get; private set; }
        public void Parse(string input)
        {
            var parts = input.Split(separators).ToList();
            UserPreviousCommand = string.Join(' ', parts);

            _path = parts[0];
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