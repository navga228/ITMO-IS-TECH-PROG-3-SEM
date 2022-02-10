using Backups;

namespace BackupsExtra
{
    public interface IRepositoryExtra : IRepository
    {
        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint);
        public void ExtractFileFromAcrchive(string archivePath, string jobObjectName, string destination);
        public void ExtrractFilesFromSplit(string archiveDirectory, string jobObjectName, string destination);
    }
}