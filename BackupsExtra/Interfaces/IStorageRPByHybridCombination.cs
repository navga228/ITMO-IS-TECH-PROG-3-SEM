using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public interface IStorageRPByHybridCombination
    {
        public List<RestorePoint> Select(BackupJobExtra backupJobExtra);
    }
}