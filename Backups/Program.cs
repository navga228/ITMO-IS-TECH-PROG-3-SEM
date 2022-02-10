using System.IO;
using System.IO.Compression;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            LocalFilesRepository repository = new LocalFilesRepository(@"/Users/navga228/Desktop/BackupFiles/");
            SingleStorageAlgorithm singleAlgorithm = new SingleStorageAlgorithm();
            BackupJob backupJob = new BackupJob("BackUpJob1", string.Empty, repository, singleAlgorithm);

            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repository.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");

            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            repository.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");

            // /Users/navga228/BackUpJob1/JobObject
            backupJob.AddJobObject(file1);
            backupJob.AddJobObject(file2);

            backupJob.BackupProcessing("RestorePoint1");
        }
    }
}
