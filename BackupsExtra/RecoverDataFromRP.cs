using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class RecoverDataFromRP
    {
        private IRepositoryExtra _repository;

        public RecoverDataFromRP(IRepositoryExtra repository)
        {
            if (repository == null)
            {
                throw new BackupsExtraException("repository is null!");
            }

            _repository = repository;
        }

        public void RecoverToOriginalLocation(BackupJobExtra backupJobExtra, RestorePointExtra restorePointExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (restorePointExtra == null)
            {
                throw new BackupsExtraException("restorePointExtra is null!");
            }

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
                    _repository.ExtractFilesFromSplit(backupJobName + "/" + restorePointName, jobObject.Name, jobObject.FilePath);
                }
            }
        }

        public void RecoverToDifferentLocation(BackupJobExtra backupJobExtra, RestorePointExtra restorePointExtra, string path)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (restorePointExtra == null)
            {
                throw new BackupsExtraException("restorePointExtra is null!");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("path is null or empty!");
            }

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
                    _repository.ExtractFilesFromSplit(backupJobName + "/" + restorePointName, jobObject.Name, path + "/" + jobObject.Name);
                }
            }
        }
    }
}