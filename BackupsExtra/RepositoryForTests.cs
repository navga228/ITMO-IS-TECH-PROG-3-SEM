using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class RepositoryForTests
    {
        private string _root = "/Users/navga228/Desktop/3лабаООП";
        public Dictionary<string, List<string>> FileSystem { get; } = new Dictionary<string, List<string>>(); // Ключ это путь до папки а значение файлы

        // Будем проверять кол-во файлов в системе
        public void CreateFile(string path, string fileName)
        {
            if (!FileSystem.ContainsKey(path))
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
            string newPath = path.Substring(0, path.Length - 1);
            if (FileSystem.ContainsKey(_root + "/" + newPath))
            {
                FileSystem[_root + "/" + newPath].Remove(fileName);
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
            if (!FileSystem.ContainsKey(_root + "/" + backupJobName + "/" + restorePointName))
            {
                FileSystem.Add(_root + "/" + backupJobName + "/" + restorePointName, new List<string>());
                FileSystem[_root + "/" + backupJobName + "/" + restorePointName].Add(jobObject.Name);
            }
            else
            {
                FileSystem[_root + "/" + backupJobName + "/" + restorePointName].Add(jobObject.Name);
            }
        }

        public void MakeArchive(string pathToDirectory, string newArchiveFileName)
        {
            FileSystem[newArchiveFileName].Add(pathToDirectory);
        }

        public void CopyFile(string path, string newPath)
        {
            string filePath;
            int lastSlash;
            lastSlash = newPath.LastIndexOf("/");
            filePath = newPath.Substring(0, lastSlash);
            string[] words = newPath.Split('/');
            FileSystem[filePath].Add(words[words.Length - 1]); // то есть сохраняется и путь в систему как ключ и добавляется файл в лист той папки где он лежит
        }
    }
}