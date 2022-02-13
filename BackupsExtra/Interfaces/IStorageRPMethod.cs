using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public interface IStorageRPMethod
    {
        public List<RestorePoint> Select(BackupJobExtra backupJobExtra);
    }
}