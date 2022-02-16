using System;
using System.IO;
using Backups;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            // Тест восстановление из ресторпоинта
            FileLog fileLog = new FileLog(@"/Users/navga228/Desktop/BackupFiles/logs", true);
            LocalFilesRepositoryExtra localFilesRepositoryExtra = new LocalFilesRepositoryExtra(@"/Users/navga228/Desktop/BackupFiles/", fileLog);

            SplitStoragesAlgorithmExtra splitStoragesAlgorithmExtra = new SplitStoragesAlgorithmExtra(new SplitStoragesAlgorithm());

            SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(5);
            DeleteRestorPoints deleteRestorPoints = new DeleteRestorPoints();
            BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", string.Empty, splitStoragesAlgorithmExtra, localFilesRepositoryExtra, fileLog, selectRpByQuantity, deleteRestorPoints);

            JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            localFilesRepositoryExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            File.AppendAllText(@"/Users/navga228/Desktop/BackupFiles/JobObject1", "JobObject1");

            JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            localFilesRepositoryExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            File.AppendAllText(@"/Users/navga228/Desktop/BackupFiles/JobObject2", "JobObject2");

            backupJobExtra.AddJobObject(file1);
            backupJobExtra.AddJobObject(file2);

            backupJobExtra.BackupProcessing("RestorePoint1");

            // localFilesRepositoryExtra.DeleteFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            // localFilesRepositoryExtra.DeleteFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            RecoverDataFromRP recoverDataFromRp = new RecoverDataFromRP(localFilesRepositoryExtra);

            // Восстановдление в прежднее место
            // recoverDataFromRp.RecoverToOriginalLocation(backupJobExtra, backupJobExtra.GetBackupJob.RestorePoints[0]);

            // Восстановление в новое место
            recoverDataFromRp.RecoverToDifferentLocation(backupJobExtra, backupJobExtra.GetBackupJob.RestorePoints[0], localFilesRepositoryExtra.GetRoot() + "test");

            // Тест для Сохранения и загрузки данных(сериализация/десеиализация)
            // FileLog fileLog = new FileLog(@"/Users/navga228/Desktop/BackupFiles/logs", true);
            // LocalFilesRepositoryExtra localFilesRepositoryExtra = new LocalFilesRepositoryExtra(@"/Users/navga228/Desktop/BackupFiles/", fileLog);
            //
            // SplitStoragesAlgorithmExtra splitStoragesAlgorithmExtra = new SplitStoragesAlgorithmExtra(new SplitStoragesAlgorithm());
            //
            // SelectRPByQuantity selectRpByQuantity = new SelectRPByQuantity(5);
            // DeleteRestorPoints deleteRestorPoints = new DeleteRestorPoints();
            // BackupJobExtra backupJobExtra = new BackupJobExtra("BackupJob1", string.Empty, splitStoragesAlgorithmExtra, localFilesRepositoryExtra, fileLog, selectRpByQuantity, deleteRestorPoints);
            //
            // JobObject file1 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            // localFilesRepositoryExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject1");
            // File.AppendAllText(@"/Users/navga228/Desktop/BackupFiles/JobObject1", "JobObject1");
            //
            // JobObject file2 = new JobObject(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            // localFilesRepositoryExtra.CreateFile(@"/Users/navga228/Desktop/BackupFiles/", "JobObject2");
            // File.AppendAllText(@"/Users/navga228/Desktop/BackupFiles/JobObject2", "JobObject2");
            //
            // backupJobExtra.AddJobObject(file1);
            // backupJobExtra.AddJobObject(file2);
            //
            // backupJobExtra.BackupProcessing("RestorePoint1");
            // BackupJobExtra newBackupJobExtra = localFilesRepositoryExtra.GetData("BackupJob1");
            // Console.WriteLine(newBackupJobExtra.GetBackupJob.Name);
        }
    }
}
