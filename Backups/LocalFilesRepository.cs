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

        public void CreateFile(string root, string fileName)
        {
            if (!File.Exists(_root + root + fileName))
            {
                File.CreateText(_root + root + fileName);
            }
        }

        public void DeleteFile(string path)
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

        public void DeleteDerictory(string path)
        {
            if (Directory.Exists(_root + path))
            {
                Directory.CreateDirectory(_root + path);
            }
        }

        public void CompressFiles(List<JobObject> jobObjects, string pathToDerictoryWithCompressedFiles)
        {
            foreach (var jobObject in jobObjects)
            {
                using (FileStream sourceStream = new FileStream(jobObject.FilePath, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(_root + pathToDerictoryWithCompressedFiles))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        }
                    }
                }
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
                File.Copy(path, _root + newPath);
            }
        }
    }
}