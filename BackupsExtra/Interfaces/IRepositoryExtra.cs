using Backups;

namespace BackupsExtra
{
    public interface IRepositoryExtra : IRepository
    {
        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint);
        public void ExtractFileFromAcrchive(string archivePath, string jobObjectName, string destination);
        public void ExtractFilesFromSplit(string archiveDirectory,  string jobObjectName, string destination);
        public void SaveData(BackupJobExtra backupJobExtra);
        public BackupJobExtra GetData(string backupJobExtraName);
    }
}