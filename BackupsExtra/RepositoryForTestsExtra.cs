using System.Collections.Generic;
using System.IO;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class RepositoryForTestsExtra : IRepositoryExtra
    {
        private string _root = "/Users/navga228/Desktop/BackupFiles/";

        public RepositoryForTestsExtra(string root)
        {
            _root = root;
        }

        public Dictionary<string, List<string>> FileSystem { get; } = new Dictionary<string, List<string>>(); // Ключ это полный путь до директории, а значение файлы в этой директории

        public void CreateFile(string path, string fileName)
        {
            if (FileSystem.ContainsKey(path))
            {
                List<string> file = new List<string>();
                file.Add(fileName);
                FileSystem[path].AddRange(file);
            }
            else
            {
                FileSystem.Add(path, new List<string>());
                List<string> file = new List<string>();
                file.Add(fileName);
                FileSystem[path].AddRange(file);
            }
        }

        public void DeleteFile(string path, string fileName)
        {
            if (FileSystem.ContainsKey(_root + path))
            {
                if (FileSystem[_root + path].Contains(fileName))
                {
                    FileSystem[_root + path].Remove(fileName);
                }
            }
        }

        public void CreateDirectory(string path, string directoryName)
        {
            List<string> directory = new List<string>();
            FileSystem.Add(path + directoryName, directory);
        }

        public void DeleteDirectory(string path, string directoryName)
        {
            if (FileSystem.ContainsKey(path))
            {
                FileSystem[path].Remove(directoryName);
            }
        }

        public void CompressFiles(JobObject jobObject, string restorePointName, string backupJobName)
        {
            if (!FileSystem.ContainsKey(_root + backupJobName + "/" + restorePointName))
            {
                FileSystem.Add(_root + backupJobName + "/" + restorePointName, new List<string>());
                FileSystem[_root + backupJobName + "/" + restorePointName].Add(jobObject.Name + ".zip");
                FileSystem.Add(_root + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", new List<string>());
                FileSystem[_root + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip"].Add(jobObject.Name);
            }
            else
            {
                FileSystem[_root + backupJobName + "/" + restorePointName].Add(jobObject.Name + ".zip");
                FileSystem.Add(_root + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", new List<string>());
                FileSystem[_root + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip"].Add(jobObject.Name);
            }
        }

        public void MakeArchive(string pathToDirectory, string newArchiveFileName)
        {
            string nameOfFile;
            int lastSlash;
            lastSlash = (_root + newArchiveFileName).LastIndexOf("/");
            nameOfFile = (_root + newArchiveFileName).Substring(lastSlash + 1);
            string newArchiveFileNameWithoutName = (_root + newArchiveFileName).Substring(0, lastSlash);
            if (FileSystem.ContainsKey(newArchiveFileNameWithoutName))
            {
                FileSystem[newArchiveFileNameWithoutName].Add(nameOfFile);
                FileSystem.Add(_root + newArchiveFileName, new List<string>());
                foreach (var file in FileSystem[_root + pathToDirectory])
                {
                    FileSystem[_root + newArchiveFileName].Add(file);
                }
            }
            else
            {
                FileSystem.Add(newArchiveFileNameWithoutName, new List<string>());
                FileSystem[newArchiveFileNameWithoutName].Add(nameOfFile);
                FileSystem.Add(_root + newArchiveFileName, new List<string>());
                foreach (var file in FileSystem[_root + pathToDirectory])
                {
                    FileSystem[_root + newArchiveFileName].Add(file);
                }
            }
        }

        public void CopyFile(string filePath, string destination)
        {
            string nameOfFile;
            int lastSlash;
            lastSlash = destination.LastIndexOf("/");
            nameOfFile = destination.Substring(lastSlash + 1);
            string destinationWithoutName = destination.Substring(0, lastSlash);
            if (FileSystem.ContainsKey(destinationWithoutName))
            {
                FileSystem[destinationWithoutName].Add(nameOfFile);
            }
            else
            {
                FileSystem.Add(destinationWithoutName, new List<string>());
                FileSystem[destinationWithoutName].Add(nameOfFile);
            }
        }

        public void DeleteRestorePoints(BackupJobExtra backupJobExtra, RestorePoint restorePoint)
        {
            FileSystem[_root + backupJobExtra.GetBackupJob.Name].Remove(restorePoint.Name);
        }

        public void ExtractFilesToTemporaryDirectory(string archivePath, string destination)
        { // Разархивирует файлы во временную директорию, из которой потом они будут распрделяться в нужные места
            if (FileSystem.ContainsKey(_root + destination))
            {
                foreach (var file in FileSystem[_root + archivePath])
                {
                    FileSystem[_root + destination].Add(file);
                }
            }
            else
            {
                FileSystem.Add(_root + destination, new List<string>());
                foreach (var file in FileSystem[_root + archivePath])
                {
                    FileSystem[_root + destination].Add(file);
                }
            }
        }

        public void ExtractFilesToDirectory(string archivePath, string destination)
        {
            // Вытаскивает из архива файл и восстанавливает его по относительному пути
            if (FileSystem.ContainsKey(destination))
            {
                foreach (var file in FileSystem[_root + archivePath])
                {
                    FileSystem[destination].Add(file);
                }
            }
            else
            {
                FileSystem.Add(destination, new List<string>());
                foreach (var file in FileSystem[_root + archivePath])
                {
                    FileSystem[destination].Add(file);
                }
            }
        }

        public string GetRoot()
        {
            return _root;
        }

        public void SaveData(BackupJobExtra backupJobExtra)
        {
            return;
        }

        public BackupJobExtra GetData(string backupJobExtraName)
        {
            return null;
        }

        public List<string> EnumerateFiles(string path)
        {
            List<string> answer = new List<string>();
            if (FileSystem.ContainsKey(path))
            {
                foreach (var file in FileSystem[path])
                {
                    answer.Add(path + "/" + file);
                }
            }
            else
            {
                FileSystem.Add(path, new List<string>());
                foreach (var file in FileSystem[path])
                {
                    answer.Add(path + file);
                }
            }

            return answer;
        }
    }
}