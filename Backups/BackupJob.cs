using System;
using System.Collections.Generic;
using Backups.Tools;

namespace Backups
{
    public class BackupJob
    {
      public BackupJob(string name, string projectPath, IRepository repository, IBackupAlgorithm backupAlgorithm)
      {
          Repository = repository;
          ProjectPath = projectPath;
          Name = name;
          BackupAlgorithm = backupAlgorithm;
          JobObjects = new List<JobObject>();
          RestorePoints = new List<RestorePoint>();
          Repository.CreateDirectory(projectPath, name);
          Repository.CreateDirectory(projectPath + name + "/", "JobObject");
      }

      public string Name { get; }
      public List<JobObject> JobObjects { get; }
      public List<RestorePoint> RestorePoints { get; }
      public IBackupAlgorithm BackupAlgorithm { get; }
      public IRepository Repository { get; }
      public string ProjectPath { get; }
      public void AddJobObject(JobObject jobObject)
      {
          if (jobObject == null)
          {
              throw new BackupsException("jobObject is null");
          }

          Repository.CopyFile(jobObject.FilePath, ProjectPath + Name + "/JobObject/" + jobObject.Name);
          JobObjects.Add(jobObject);
      }

      public void RemoveJobObject(JobObject jobObject)
      {
          if (jobObject == null)
          {
              throw new BackupsException("jobObject is null");
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