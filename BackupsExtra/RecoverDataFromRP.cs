using Backups;

namespace BackupsExtra
{
    public class RecoverDataFromRP
    {
        private IRepositoryExtra _repository;

        public RecoverDataFromRP(IRepositoryExtra repository)
        {
            _repository = repository;
        }

        public void RecoverToOriginalLocation(BackupJobExtra backupJobExtra, RestorePointExtra restorePointExtra)
        {
            string restorePointName = restorePointExtra.GetRestorePoint.Name;
            string backupJobName = backupJobExtra.GetBackupJob.Name;
            if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithm)
            {
                foreach (var jobObject in restorePointExtra.GetRestorePoint.BachupedFiles)
                {
                    _repository.ExtractFileFromAcrchive(backupJobName + "/" + restorePointName + "/" + restorePointName + ".zip", jobObject.Name, jobObject.FilePath);
                }
            }
            else
            {
                foreach (var jobObject in restorePointExtra.GetRestorePoint.BachupedFiles)
                {
                    _repository.ExtrractFilesFromSplit(backupJobName + "/" + restorePointName, jobObject.Name, jobObject.FilePath);
                }
            }
        }

        public void RecoverToDifferentLocation(BackupJobExtra backupJobExtra, RestorePointExtra restorePointExtra, string path)
        {
            string restorePointName = restorePointExtra.GetRestorePoint.Name;
            string backupJobName = backupJobExtra.GetBackupJob.Name;
            if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithm)
            {
                foreach (var jobObject in restorePointExtra.GetRestorePoint.BachupedFiles)
                {
                    _repository.ExtractFileFromAcrchive(backupJobName + "/" + restorePointName + "/" + restorePointName + ".zip", jobObject.Name, path + "/" + jobObject.Name);
                }
            }
            else
            {
                foreach (var jobObject in restorePointExtra.GetRestorePoint.BachupedFiles)
                {
                    _repository.ExtrractFilesFromSplit(backupJobName + "/" + restorePointName, jobObject.Name, path + "/" + jobObject.Name);
                }
            }
        }
    }
}