using System;
using System.Linq;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class BackupJobExtra
    {
        private BackupJob _backupJob;
        private IRepositoryExtra _repositoryExtra;
        private ILog _logger;
        public BackupJobExtra(string name, string projectPath, IRepository repository, IBackupAlgorithm backupAlgorithm, IRepositoryExtra repositoryExtra, ILog logger)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BackupsExtraException("Name is null or empty");
            }

            if (projectPath == null)
            {
                throw new BackupsExtraException("Project path is null");
            }

            if (repository == null)
            {
                throw new BackupsExtraException("Repository is null");
            }

            if (backupAlgorithm == null)
            {
                throw new BackupsExtraException("Backup Algorithm is null");
            }

            if (repositoryExtra == null)
            {
                throw new BackupsExtraException("RepositoryExtra is null");
            }

            if (logger == null)
            {
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
                throw new BackupsExtraException("newRestorePointName is null or empty");
            }

            _backupJob.BackupProcessing(newRestorePointName);
            _logger.Print($"{InfoAboutClass()} Message: Restore point was successfully created!");
        }

        public void DeleteRestorePoints(string restorePointName)
        {
            if (string.IsNullOrEmpty(restorePointName))
            {
                throw new BackupsExtraException("restorePointName is null or empty");
            }

            foreach (var restorePoint in _backupJob.RestorePoints.Where(restorePoint => restorePoint.Name.Equals(restorePointName)))
            {
                _backupJob.RestorePoints.Remove(restorePoint);
                _repositoryExtra.DeleteRestorePoints(this, restorePoint);
                break;
            }

            _logger.Print($"{InfoAboutClass()} Message: Restore points was successfully deleted!");
        }

        public void MergeRestorePoints(RestorePoint oldRestorePoint, RestorePoint newRestorePoint)
        {
            if (oldRestorePoint == null)
            {
                throw new BackupsExtraException("oldRestorePoint is null");
            }

            if (newRestorePoint == null)
            {
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

            _logger.Print($"{InfoAboutClass()} Message: Restore points was successfully merged!");
        }

        private string InfoAboutClass()
        {
            return $"Object name: {this.GetType().Name}";
        }
    }
}