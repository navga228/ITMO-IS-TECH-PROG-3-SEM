using System.IO;
using System.IO.Compression;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            LocalFilesRepository repository = new LocalFilesRepository(@"/Users/navga228/");
            SingleStorageAlgorithm singleAlgorithm = new SingleStorageAlgorithm();
            BackupJob backupJob = new BackupJob("BackUpJob1", repository, singleAlgorithm);

            JobObject file1 = new JobObject(@"/Users/navga228/BackUpJob1/JobObject/", "JobObject1");
            repository.CreateFile(string.Empty, "JobObject1");

            JobObject file2 = new JobObject(@"/Users/navga228/BackUpJob1/JobObject/", "JobObject2");
            repository.CreateFile(string.Empty, "JobObject2");

            // /Users/navga228/BackUpJob1/JobObject
            backupJob.AddJobObject(file1);
            backupJob.AddJobObject(file2);

            backupJob.BackupProcessing("RestorePoint1");
        }
    }
}
