using Backups;

namespace BackupsExtra
{
    public interface IRepositoryExtra : IRepository
    {
        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint);
        public void ExtractFilesToTemporaryDirectory(string archivePath, string destination);
        public void ExtractFilesToDirectory(string archiveDirectory, string destination);
        public void SaveData(BackupJobExtra backupJobExtra);
        public BackupJobExtra GetData(string backupJobExtraName);
    }
}