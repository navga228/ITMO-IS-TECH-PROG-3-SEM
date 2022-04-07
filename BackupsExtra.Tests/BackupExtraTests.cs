using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BackupsExtra.Tests
{
    public class BackupExtraTests
    {
        [Test]
        public void SingleStorageTest()
        {
            // Создаем логер и репозиторий для тестов
            ConsoleLog consoleLog = new ConsoleLog(true);
            RepositoryForTestsExtra repositoryForTestsExtra = new RepositoryForTestsExtra(@"/Users/navga228/Desktop/BackupFiles/");

            // Указываем сингл метод для создания рестор поинтов(рп)
            SingleStorageAlgorithmExtra singleStorageAlgorithmExtra = new SingleStorageAlgorithmExtra(new SingleStorageAlgorithm());
            
            // Указывваем хранение рп по количеству(5 штук)
            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(5);
            
            // Указываем метод удаления рп в случае их не попадания под лимит
            DeleteRestorPoints deleteRestorPoints = new DeleteRestorPoints();
            
            // Создаем джобу
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", @"/Users/navga228/Desktop/BackupFiles/", singleStorageAlgorithmExtra, repositoryForTestsExtra, consoleLog, selectRpByQuantity, deleteRestorPoints);
            
            // Создаем первый объект джобы
            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            
            // Создаем второй объект джобы
            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            
            // Добавляем их в джобу
            backupJobExtra.AddJobObject(file1);
            backupJobExtra.AddJobObject(file2);
            
            // Запускаем алгорит создания рп
            backupJobExtra.BackupProcessing("RestorePoint1");

            // Проверяем что в рп один архив, а не два как это было бы в split
            Assert.AreEqual(1, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/RestorePoint1"].Count);
        }
        [Test]
        public void SplitStoragesTest()
        {
            ConsoleLog consoleLog = new ConsoleLog(true);
            RepositoryForTestsExtra repositoryForTestsExtra = new RepositoryForTestsExtra(@"/Users/navga228/Desktop/BackupFiles/");
            
            SplitStoragesAlgorithmExtra splitStoragesAlgorithmExtra = new SplitStoragesAlgorithmExtra(new SplitStoragesAlgorithm());
            
            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(5);
            DeleteRestorPoints deleteRestorPoints = new DeleteRestorPoints();
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", @"/Users/navga228/Desktop/BackupFiles/", splitStoragesAlgorithmExtra, repositoryForTestsExtra, consoleLog, selectRpByQuantity, deleteRestorPoints);
            
            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            
            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");

            backupJobExtra.AddJobObject(file1);
            backupJobExtra.AddJobObject(file2);
            
            backupJobExtra.BackupProcessing("RestorePoint1");

            backupJobExtra.GetBackupJob.RemoveJobObject(file1);

            backupJobExtra.BackupProcessing("RestorePoint2");

            // Проверяем что в файловой системе сейчас один файл в jobObject тк второй мы удалили
            Assert.AreEqual(1, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/JobObject"].Count);
            // Проверяем что при создании первого ресторпоинта бало забекаплено 2 файла
            Assert.AreEqual(2, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/RestorePoint1"].Count);
            // Проверяем что после удаление одного файла забекапится только один файл
            Assert.AreEqual(1, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/RestorePoint2"].Count);
        }
        [Test]
        public void MergePoints()
        {
            ConsoleLog consoleLog = new ConsoleLog(true);
            RepositoryForTestsExtra repositoryForTestsExtra = new RepositoryForTestsExtra(@"/Users/navga228/Desktop/BackupFiles/");
            
            SplitStoragesAlgorithmExtra splitStoragesAlgorithmExtra = new SplitStoragesAlgorithmExtra(new SplitStoragesAlgorithm());
            
            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(1);
            MergeRestorePoints mergeRestorePoints = new MergeRestorePoints(repositoryForTestsExtra);
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", @"/Users/navga228/Desktop/BackupFiles/", splitStoragesAlgorithmExtra, repositoryForTestsExtra, consoleLog, selectRpByQuantity, mergeRestorePoints);
            
            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");

            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");

            backupJobExtra.AddJobObject(file1);
            
            backupJobExtra.BackupProcessing("RestorePoint1");
            // Проверяем что действительно в первом рп один файл
            List<string> jobObject1 = new List<string>();
            jobObject1.Add("JobObject1.zip");
            Assert.AreEqual(jobObject1, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/RestorePoint1"]);

            backupJobExtra.AddJobObject(file2);
            backupJobExtra.BackupProcessing("RestorePoint2");
            
            // Тк ограничение по точкам стоит единица, то при создании второго рп будет первая точка вмерджена во вторую. Проверяем что после мерджа у нас во втором рп стало два файла
            List<string> jobObject1and2 = new List<string>();
            jobObject1and2.Add("JobObject1.zip");
            jobObject1and2.Add("JobObject2.zip");
            Assert.AreEqual(jobObject1and2, repositoryForTestsExtra.FileSystem["/Users/navga228/Desktop/BackupFiles/BackupJob1/RestorePoint2"]);
        }

        [Test]
        public void RecoverFilesToDifferentLocationForSingle()
        { // тестирую только differentLocation тк в для воосстановления в пржднее значение просто меняется path
            ConsoleLog consoleLog = new ConsoleLog(true);
            RepositoryForTestsExtra repositoryForTestsExtra = new RepositoryForTestsExtra(@"/Users/navga228/Desktop/BackupFiles/");

            SingleStorageAlgorithmExtra singleStorageAlgorithmExtra = new SingleStorageAlgorithmExtra(new SingleStorageAlgorithm());
            
            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(1);
            MergeRestorePoints mergeRestorePoints = new MergeRestorePoints(repositoryForTestsExtra);
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", @"/Users/navga228/Desktop/BackupFiles/", singleStorageAlgorithmExtra, repositoryForTestsExtra, consoleLog, selectRpByQuantity, mergeRestorePoints);
            
            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");

            backupJobExtra.AddJobObject(file1);
            
            backupJobExtra.BackupProcessing("RestorePoint1");

            RecoverDataFromRP recoverDataFromRp = new RecoverDataFromRP(repositoryForTestsExtra);
            recoverDataFromRp.RecoverToDifferentLocation(backupJobExtra, backupJobExtra.GetBackupJob.RestorePoints[0], repositoryForTestsExtra.GetRoot() + "newLocation");
            List<string> jobObject1 = new List<string>();
            jobObject1.Add("JobObject1");
            // Проверяем что действительно файлы восстановились в нужное место
            Assert.AreEqual(jobObject1, repositoryForTestsExtra.FileSystem[repositoryForTestsExtra.GetRoot() + "newLocation"]);
        }

        [Test]
        public void RecoverFilesToDifferentLocationForSplit()
        {
            ConsoleLog consoleLog = new ConsoleLog(true);
            RepositoryForTestsExtra repositoryForTestsExtra = new RepositoryForTestsExtra(@"/Users/navga228/Desktop/BackupFiles/");

            SplitStoragesAlgorithmExtra splitStoragesAlgorithmExtra = new SplitStoragesAlgorithmExtra(new SplitStoragesAlgorithm());
            
            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(1);
            MergeRestorePoints mergeRestorePoints = new MergeRestorePoints(repositoryForTestsExtra);
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", @"/Users/navga228/Desktop/BackupFiles/", splitStoragesAlgorithmExtra, repositoryForTestsExtra, consoleLog, selectRpByQuantity, mergeRestorePoints);
            
            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");

            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            repositoryForTestsExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");

            backupJobExtra.AddJobObject(file1);
            backupJobExtra.AddJobObject(file2);
            
            
            backupJobExtra.BackupProcessing("RestorePoint1");

            RecoverDataFromRP recoverDataFromRp = new RecoverDataFromRP(repositoryForTestsExtra);
            recoverDataFromRp.RecoverToDifferentLocation(backupJobExtra, backupJobExtra.GetBackupJob.RestorePoints[0], repositoryForTestsExtra.GetRoot() + "newLocation");
            List<string> jobObject1and2 = new List<string>();
            jobObject1and2.Add("JobObject1");
            jobObject1and2.Add("JobObject2");
            // Проверяем что действительно два файла восстановились в нужное место
            Assert.AreEqual(jobObject1and2, repositoryForTestsExtra.FileSystem[repositoryForTestsExtra.GetRoot() + "newLocation"]);
        }
    }
}