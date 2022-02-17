using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    [Serializable]
    public class LocalFilesRepository : IRepository
    {
        private string _root;

        public LocalFilesRepository(string root)
        {
            _root = root;
        }

        public string GetRoot()
        {
            return _root;
        }

        public void CreateFile(string path, string fileName)
        {
            if (!File.Exists(path + fileName))
            {
                File.CreateText(path + fileName);
            }
        }

        public void DeleteFile(string path, string fileName)
        {
            if (File.Exists(_root + path))
            {
                File.Delete(_root + path);
            }
        }

        public void CreateDirectory(string path, string fileName)
        {
            if (!Directory.Exists(path + fileName))
            {
                Directory.CreateDirectory(path + fileName);
            }
        }

        public void DeleteDirectory(string path, string directoryName)
        {
            if (Directory.Exists(_root + path + directoryName))
            {
                Directory.Delete(_root + path + directoryName);
            }
        }

        public void CompressFiles(JobObject jobObject, string restorePointName, string backupJobName)
        {
            // создание нового архива
            using (ZipArchive zipArchive = ZipFile.Open(_root + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", ZipArchiveMode.Create))
            {
                // вызов метода для добавления файла в архив
                zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.Name);
            }
        }

        public void MakeArchive(string backupJobName, string newArchiveFileName)
        {
            ZipFile.CreateFromDirectory(_root + backupJobName, _root + newArchiveFileName);
        }

        public void CopyFile(string path, string newPath)
        {
            if (File.Exists(path))
            {
                File.Copy(path, newPath, true);
            }
        }
    }
}