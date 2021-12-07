using System.Collections.Generic;

namespace Backups
{
    public interface IRepository
    {
        public void CreateFile(string root, string fileName);
        public void DeleteFile(string path);
        public void CreateDerictory(string root, string fileName);
        public void DeleteDerictory(string path);
        public void CompressFiles(List<JobObject> jobObjects, string pathToDerictoryWithCompressedFiles);
        public void MakeArchive(string pathToDirectory, string newArchiveFileName);
        public void CopyFile(string path, string newPath);
    }
}