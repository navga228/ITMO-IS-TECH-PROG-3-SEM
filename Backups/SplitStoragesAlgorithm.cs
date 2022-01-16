using System;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class SplitStoragesAlgorithm : IBackupAlgorithm
    {
        public RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository)
        {
            repository.CompressFiles(jobObjects, restorePointName, backupJobName);
            return new RestorePoint(restorePointName, DateTime.Now, jobObjects);
        }
    }
}