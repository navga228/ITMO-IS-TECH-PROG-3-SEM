using System.Collections.Generic;

namespace Backups
{
    public interface IRepository
    {
        void CreateFile(string path, string fileName);
        void DeleteFile(string path, string fileName);
        void CreateDerictory(string path, string derictoryName);
        void DeleteDerictory(string path, string derictoryName);
        void CompressFiles(List<JobObject> jobObjects, string restorePointName, string backupJobName);
        void MakeArchive(string pathToDirectory, string newArchiveFileName);
        void CopyFile(string path, string newPath);
    }
}