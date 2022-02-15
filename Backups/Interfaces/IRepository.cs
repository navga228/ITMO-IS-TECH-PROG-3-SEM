using System.Collections.Generic;

namespace Backups
{
    public interface IRepository
    {
        void CreateFile(string path, string fileName);
        void DeleteFile(string path, string fileName);
        void CreateDirectory(string path, string directoryName);
        void DeleteDirectory(string path, string directoryName);
        void CompressFiles(JobObject jobObject, string restorePointName, string backupJobName);
        void MakeArchive(string pathToDirectory, string newArchiveFileName);
        void CopyFile(string path, string newPath);
    }
}