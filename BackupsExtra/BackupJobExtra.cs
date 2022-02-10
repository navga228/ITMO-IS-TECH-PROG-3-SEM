using System;
using System.Linq;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class BackupJobExtra
    { // validation and logger done
        private BackupJob _backupJob;
        private IRepositoryExtra _repositoryExtra;
        private ILog _logger;
        public BackupJobExtra(string name, string projectPath, IRepository repository, IBackupAlgorithm backupAlgorithm, IRepositoryExtra repositoryExtra, ILog logger)
        {
            if (string.IsNullOrEmpty(name))
            {
                LogInfo($"{InfoAboutClass()} Message: Name is null or empty!");
                throw new BackupsExtraException("Name is null or empty");
            }

            if (projectPath == null)
            {
                LogInfo($"{InfoAboutClass()} Message: Project path is null!");
                throw new BackupsExtraException("Project path is null");
            }

            if (repository == null)
            {
                LogInfo($"{InfoAboutClass()} Message: Repository is null!");
                throw new BackupsExtraException("Repository is null");
            }

            if (backupAlgorithm == null)
            {
                LogInfo($"{InfoAboutClass()} Message: backupAlgorithm is null!");
                throw new BackupsExtraException("Backup Algorithm is null");
            }

            if (repositoryExtra == null)
            {
                LogInfo($"{InfoAboutClass()} Message: RepositoryExtra is null!");
                throw new BackupsExtraException("RepositoryExtra is null");
            }

            if (logger == null)
            {
                LogInfo($"{InfoAboutClass()} Message: Logger is null!");
                throw new BackupsExtraException("Logger is null");
            }

            _backupJob = new BackupJob(name, projectPath, repository, backupAlgorithm);
            _repositoryExtra = repositoryExtra;
            _logger = logger;
        }

        public BackupJob GetBackupJob
        {
            get => _backupJob;
        }

        public void BackupProcessing(string newRestorePointName)
        {
            if (string.IsNullOrEmpty(newRestorePointName))
            {
                LogInfo($"{InfoAboutClass()} Message: newRestorePointName is null or empty!");
                throw new BackupsExtraException("newRestorePointName is null or empty");
            }

            _backupJob.BackupProcessing(newRestorePointName);
            LogInfo($"{InfoAboutClass()} Message: Restore point was successfully created!");
        }

        public void DeleteRestorePoints(string restorePointName)
        {
            if (string.IsNullOrEmpty(restorePointName))
            {
                LogInfo($"{InfoAboutClass()} Message: RestorePointName is null or empty!");
                throw new BackupsExtraException("restorePointName is null or empty");
            }

            foreach (var restorePoint in _backupJob.RestorePoints.Where(restorePoint => restorePoint.Name.Equals(restorePointName)))
            {
                _backupJob.RestorePoints.Remove(restorePoint);
                _repositoryExtra.DeleteRestorePoints(this, restorePoint);
                break;
            }

            LogInfo($"{InfoAboutClass()} Message: Restore points was successfully deleted!");
        }

        public void MergeRestorePoints(RestorePoint oldRestorePoint, RestorePoint newRestorePoint)
        {
            if (oldRestorePoint == null)
            {
                LogInfo($"{InfoAboutClass()} Message: oldRestorePoint is null!");
                throw new BackupsExtraException("oldRestorePoint is null");
            }

            if (newRestorePoint == null)
            {
                LogInfo($"{InfoAboutClass()} Message: newRestorePoint is null!");
                throw new BackupsExtraException("newRestorePoint is null");
            }

            if (_backupJob.BackupAlgorithm is SingleStorageAlgorithm)
            {
               DeleteRestorePoints(oldRestorePoint.Name);
            }
            else
            {
                foreach (JobObject jobObject in oldRestorePoint.BachupedFiles.Where(jobObject => !newRestorePoint.BachupedFiles.Contains(jobObject)))
                {
                    newRestorePoint.BachupedFiles.Add(jobObject);
                    _repositoryExtra.CopyFile(
                        _backupJob.Name + "/" + oldRestorePoint.Name + "/" + jobObject.Name + ".zip",
                        _backupJob.Name + "/" + newRestorePoint.Name + "/" + jobObject.Name + ".zip");
                }

                DeleteRestorePoints(oldRestorePoint.Name);
            }

            LogInfo($"{InfoAboutClass()} Message: Restore points was successfully merged!");
        }

        private void LogInfo(string message)
        {
            _logger?.Print(LogLevel.Info, message);
        }

        private void LogError(string message)
        {
            _logger?.Print(LogLevel.Error, message);
        }

        private string InfoAboutClass()
        {
            return $"Object name: {this.GetType().Name}";
        }
    }
}