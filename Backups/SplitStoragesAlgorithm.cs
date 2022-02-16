using System;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    [Serializable]
    public class SplitStoragesAlgorithm : IBackupAlgorithm
    {
        public RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository)
        {
            repository.CreateDirectory(backupJobName + "/", restorePointName);
            foreach (var jobObject in jobObjects)
            {
                repository.CompressFiles(jobObject, restorePointName, backupJobName);
            }

            return new RestorePoint(restorePointName, DateTime.Now, jobObjects);
        }
    }
}