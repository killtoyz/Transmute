public enum PathEndingEntityEnum
{
    File,
    Directory,
    NotExist
}

namespace Transmute
{
    public static class PathEndingEntity
    {
        public static PathEndingEntityEnum CheckEndingEntity(string path)
        {
            if (File.Exists(path)) return PathEndingEntityEnum.File;
            if (Directory.Exists(path)) return PathEndingEntityEnum.Directory;

            return PathEndingEntityEnum.NotExist;
        }
    }
}