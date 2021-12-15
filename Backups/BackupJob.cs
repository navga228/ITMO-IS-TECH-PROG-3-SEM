using System;
using System.Collections.Generic;

namespace Backups
{
    public class BackupJob
    {
      public BackupJob(string name, IRepository repository, IBackupAlgorithm backupAlgorithm)
      {
          Repository = repository;
          Name = name;
          BackupAlgorithm = backupAlgorithm;
          JobObjects = new List<JobObject>();
          RestorePoints = new List<RestorePoint>();
          Repository.CreateDerictory(string.Empty, name); // Empty потому что мы не используем в локал файловой сист путь, он нужен для других файловых сист
          Repository.CreateDerictory(name + "/", "JobObject");
      }

      public string Name { get; }
      public List<JobObject> JobObjects { get; }
      public List<RestorePoint> RestorePoints { get; }
      public IBackupAlgorithm BackupAlgorithm { get; }
      public IRepository Repository { get; }
      public string Path { get; }
      public void AddJobObject(JobObject jobObject)
      {
          if (jobObject == null)
          {
              throw new Exception();
          }

          Repository.CopyFile(jobObject.FilePath, Name + "/JobObject/" + jobObject.Name);
          JobObjects.Add(jobObject);
      }

      public void RemoveJobObject(JobObject jobObject)
      {
          if (jobObject == null)
          {
              throw new Exception();
          }

          if (JobObjects.Contains(jobObject))
          {
              JobObjects.Remove(jobObject);
              Repository.DeleteFile(Name + "/JobObject/", jobObject.Name);
          }
      }

      public void BackupProcessing(string newRestorePointName)
      {
          RestorePoint newRestorePoint = BackupAlgorithm.CreateBackup(newRestorePointName, Name, JobObjects, Repository);
          RestorePoints.Add(newRestorePoint);
      }
    }
}