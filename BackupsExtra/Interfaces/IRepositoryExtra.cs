using System;
using System.Collections.Generic;
using System.IO;
using Backups;

namespace BackupsExtra
{
    public interface IRepositoryExtra : IRepository
    {
        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint);
        public void ExtractFilesToTemporaryDirectory(string archivePath, string destination);
        public void ExtractFilesToDirectory(string archiveDirectory, string destination);
        public List<string> EnumerateFiles(string path);
        public Stream OpenWriteStream(string path);
        public Stream OpenReadStrem(string path);
        public bool SearchFile(string path);
    }
}