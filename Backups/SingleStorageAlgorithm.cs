using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups
{
    [Serializable]
    public class SingleStorageAlgorithm : IBackupAlgorithm
    {
        public RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository)
        {
            repository.CreateDirectory(repository.GetRoot() + backupJobName + "/", restorePointName);
            repository.MakeArchive(backupJobName + "/JobObject", backupJobName + "/" + restorePointName + "/" + restorePointName + ".zip");
            return new RestorePoint(restorePointName, DateTime.Now, jobObjects);
        }
    }
}