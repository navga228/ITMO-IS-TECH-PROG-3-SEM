using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                _repository.ExtractFilesToTemporaryDirectory(backupJobName + "/" + restorePointName + "/" + restorePoint.Name + ".zip", "temporaryDirectory");
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    foreach (string file in Directory.EnumerateFiles(_repository.GetRoot() + "temporaryDirectory"))
                    {
                        string nameOfFile;
                        int lastSlash;
                        lastSlash = file.LastIndexOf("/");
                        nameOfFile = file.Substring(lastSlash + 1);
                        if (nameOfFile == jobObject.Name)
                        {
                            _repository.CopyFile(file, jobObject.FilePath);
                        }
                    }
                }

                _repository.DeleteDirectory(_repository.GetRoot(), "temporaryDirectory");
            }
            else
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    string pathWithoutName = jobObject.FilePath.Substring(0, jobObject.FilePath.Length - jobObject.Name.Length - 1); // Тк восстановить нужно в место где раньше хранился файл, то у абсолютного пути отрезаем имя файла и поолучаем относительный путь в который и восстанавливаем файлы
                    _repository.ExtractFilesToDirectory(backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", pathWithoutName);
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
                _repository.ExtractFilesToTemporaryDirectory(backupJobName + "/" + restorePointName + "/" + restorePoint.Name + ".zip", "temporaryDirectory");
                foreach (string file in Directory.EnumerateFiles(_repository.GetRoot() + "temporaryDirectory"))
                {
                    string nameOfFile;
                    int lastSlash;
                    lastSlash = file.LastIndexOf("/");
                    nameOfFile = file.Substring(lastSlash + 1);
                    _repository.CopyFile(file, path + "/" + nameOfFile);
                }

                _repository.DeleteDirectory(_repository.GetRoot(), "temporaryDirectory");
            }
            else
            {
                foreach (var jobObject in restorePoint.BachupedFiles)
                {
                    _repository.ExtractFilesToDirectory(backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", path); // возможно джоб обждект удалить он лишний
                }
            }
        }
    }
}