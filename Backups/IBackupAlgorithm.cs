using System.Collections.Generic;

namespace Backups
{
    public interface IBackupAlgorithm
    {
        public RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository);
    }
}