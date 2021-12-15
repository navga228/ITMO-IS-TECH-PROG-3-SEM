using System.Collections.Generic;

namespace Backups
{
    public interface IBackupAlgorithm
    {
        RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository);
    }
}