using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class LocalFilesRepositoryExtra : IRepositoryExtra
    { // Validation done byt log no
        private string _root;
        private LocalFilesRepository _localFilesRepository;
        private ILog _logger;

        public LocalFilesRepositoryExtra(string root, ILog logger)
        {
            if (string.IsNullOrEmpty(root))
            {
                throw new BackupsExtraException("Message is null or empty!");
            }

            if (logger == null)
            {
                throw new BackupsExtraException("Logger is null!");
            }

            _localFilesRepository = new LocalFilesRepository(root);
            _root = root;
            _logger = logger;
        }

        public string GetRoot()
        {
            return _root;
        }

        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (restorePoint == null)
            {
                throw new BackupsExtraException("RestorePoint is null!");
            }

            Directory.Delete(_root + backupJobExtra.GetBackupJob.Name + "/" + restorePoint.Name);
            _logger.Print($"{InfoAboutClass()} Message: Restore point was successfully deleted!");
        }

        public void CreateFile(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new BackupsExtraException("File name is null or empty!");
            }

            _localFilesRepository.CreateFile(path, fileName);
            _logger.Print($"{InfoAboutClass()} Message: File was successfully created!");
        }

        public void DeleteFile(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new BackupsExtraException("File name is null or empty!");
            }

            if (File.Exists(path + "/" + fileName))
            {
                File.Delete(path + "/" + fileName);
            }

            _localFilesRepository.DeleteFile(path, fileName);
            _logger.Print($"{InfoAboutClass()} Message: File was successfully deleted!");
        }

        public void CreateDerictory(string path, string derictoryName)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            if (string.IsNullOrEmpty(derictoryName))
            {
                throw new BackupsExtraException("DerictoryName is null or empty!");
            }

            _localFilesRepository.CreateDerictory(path, derictoryName);
            _logger.Print($"{InfoAboutClass()} Message: Derictory was successfully created!");
        }

        public void DeleteDerictory(string path, string derictoryName)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            if (string.IsNullOrEmpty(derictoryName))
            {
                throw new BackupsExtraException("DerictoryNeme name is null or empty!");
            }

            _localFilesRepository.DeleteDerictory(path, derictoryName);
            _logger.Print($"{InfoAboutClass()} Message: Derictory was successfully deleted!");
        }

        public void CompressFiles(List<JobObject> jobObjects, string restorePointName, string backupJobName)
        {
            if (jobObjects == null)
            {
                throw new BackupsExtraException("JobObjects is null!");
            }

            if (string.IsNullOrEmpty(restorePointName))
            {
                throw new BackupsExtraException("restorePointName is null or empty");
            }

            if (string.IsNullOrEmpty(backupJobName))
            {
                throw new BackupsExtraException("backupJobName is null or empty");
            }

            _localFilesRepository.CompressFiles(jobObjects, restorePointName, backupJobName);
            _logger.Print($"{InfoAboutClass()} Message: Files was successfully comressed!");
        }

        public void MakeArchive(string pathToDirectory, string newArchiveFileName)
        {
            if (string.IsNullOrEmpty(pathToDirectory))
            {
                throw new BackupsExtraException("pathToDirectory is null or empty");
            }

            if (string.IsNullOrEmpty(newArchiveFileName))
            {
                throw new BackupsExtraException("newArchiveFileName is null or empty");
            }

            _localFilesRepository.MakeArchive(pathToDirectory, newArchiveFileName);
            _logger.Print($"{InfoAboutClass()} Message: Archive was successfully made!");
        }

        public void CopyFile(string path, string newPath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty");
            }

            if (string.IsNullOrEmpty(newPath))
            {
                throw new BackupsExtraException("newPath is null or empty");
            }

            File.Copy(_root + "/" + path, _root + "/" + newPath);
            _logger.Print($"{InfoAboutClass()} Message: File was successfully copied!");
        }

        public void ExtractFileFromAcrchive(string archivePath, string jobObjectName, string destination)
        { // Вытаскивает по одному файлу из архива
            using (ZipArchive zipArchive = ZipFile.Open(_root + "/" + archivePath, ZipArchiveMode.Read))
            {
                foreach (var file in zipArchive.Entries)
                {
                    if (file.Name.Equals(jobObjectName))
                    {
                        if (File.Exists(destination))
                        {
                            File.Delete(destination);
                        }

                        file.ExtractToFile(destination);
                    }
                }
            }
        }

        public void ExtrractFilesFromSplit(string archiveDirectory, string jobObjectName, string destination)
        {
            foreach (string file in Directory.EnumerateFiles(_root + "/" + archiveDirectory))
            {
                if (!file.Equals(jobObjectName + ".zip")) continue;
                ZipFile.ExtractToDirectory(
                    _root + "/" + archiveDirectory + "/" + file,
                    destination);
                break;
            }
        }

        private string InfoAboutClass()
        {
            return $"Object name: {this.GetType().Name}";
        }
    }
}