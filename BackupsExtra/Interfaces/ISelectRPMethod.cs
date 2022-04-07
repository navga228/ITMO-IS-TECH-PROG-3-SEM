using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public interface ISelectRPMethod
    {
        public List<RestorePoint> Select(BackupJobExtra backupJobExtra);
    }
}