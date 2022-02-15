using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    public class LocalFilesRepository : IRepository
    {
        private string _root;

        public LocalFilesRepository(string root)
        {
            _root = root;
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

        public void CreateDerictory(string path, string fileName)
        {
            if (!Directory.Exists(_root + path + fileName))
            {
                Directory.CreateDirectory(_root + path + fileName);
            }
        }

        public void DeleteDerictory(string path, string derictoryName)
        {
            if (Directory.Exists(_root + path + derictoryName))
            {
                Directory.Delete(_root + path + derictoryName);
            }
        }

        public void CompressFiles(JobObject jobObject, string restorePointName, string backupJobName)
        {
            // создание нового архива
            using (ZipArchive zipArchive = ZipFile.Open(_root + "/" + backupJobName + "/" + restorePointName + "/" + jobObject.Name + ".zip", ZipArchiveMode.Create))
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
                File.Copy(path, _root + newPath, true);
            }
        }
    }
}