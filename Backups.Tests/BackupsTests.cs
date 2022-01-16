using System.Collections.Generic;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupsTests
    {
        [Test]
        public void SplitStoragesTest()
        {
            RepositoryForTests repository = new RepositoryForTests();
            SplitStoragesAlgorithm splitStorageAlgorithm = new SplitStoragesAlgorithm();
            BackupJob backupJob = new BackupJob("BackUpJob1", "/Users/navga228/Desktop/3лабаООП/", repository, splitStorageAlgorithm);

            JobObject file1 = new JobObject("/Users/navga228/Desktop/3лабаООП/", "JobObject1");

            JobObject file2 = new JobObject("/Users/navga228/Desktop/3лабаООП/", "JobObject2");
            
            backupJob.AddJobObject(file1);
            backupJob.AddJobObject(file2);

            backupJob.BackupProcessing("RestorePoint1");
            
            backupJob.RemoveJobObject(file1);
            
            backupJob.BackupProcessing("RestorePoint2");
            
            Assert.That(repository.FileSystem["/Users/navga228/Desktop/3лабаООП/BackUpJob1/JobObject"].Count == 1, Is.True); // Проверяем что в файловой системе сейчас один файл в jobObject тк второй мы удалили
            Assert.That(repository.FileSystem["/Users/navga228/Desktop/3лабаООП/BackUpJob1/RestorePoint1"].Count == 2, Is.True); // Проверяем что при создании первого ресторпоинта бало забекаплено 2 файла
            Assert.That(repository.FileSystem["/Users/navga228/Desktop/3лабаООП/BackUpJob1/RestorePoint2"].Count == 1, Is.True); // Проверяем что после удаление одного файла забекапится только один файл
        }
    }
}