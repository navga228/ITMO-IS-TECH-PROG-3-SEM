using System.IO;
using System.IO.Compression;
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

        public void RecoverToOriginalLocation(BackupJobExtra backupJobExtra, RestorePoint restorePoint)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (restorePoint == null)
            {
                throw new BackupsExtraException("restorePointExtra is null!");
            }

            string restorePointName = restorePoint.Name;
            string backupJobName = backupJobExtra.GetBackupJob.Name;
            if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithmExtra)
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    _repository.ExtractFileFromAcrchive(backupJobName + "/" + restorePointName + "/" + restorePointName + ".zip", jobObject.Name, jobObject.FilePath);
                }
            }
            else
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    string pathWithoutName = jobObject.FilePath.Substring(0, jobObject.FilePath.Length - jobObject.Name.Length - 1); // Тк восстановить нужно в место где раньше хранился файл, то у абсолютного пути отрезаем имя файла и поолучаем относительный путь в который и восстанавливаем файлы
                    _repository.ExtractFilesFromSplit(backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", jobObject.Name, pathWithoutName);
                }
            }
        }

        public void RecoverToDifferentLocation(BackupJobExtra backupJobExtra, RestorePoint restorePoint, string path)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (restorePoint == null)
            {
                throw new BackupsExtraException("restorePointExtra is null!");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("path is null or empty!");
            }

            string restorePointName = restorePoint.Name;
            string backupJobName = backupJobExtra.GetBackupJob.Name;
            if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithmExtra)
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    _repository.ExtractFilesFromSplit(backupJobName + "/" + restorePointName + "/" + restorePointName + ".zip", jobObject.Name, path);
                }
            }
            else
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    _repository.ExtractFilesFromSplit(backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", jobObject.Name, path);
                }
            }
        }
    }
}