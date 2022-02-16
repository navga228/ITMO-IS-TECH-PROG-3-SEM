using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public interface ISelectRPByHybridCombination
    {
        public List<RestorePoint> Select(BackupJobExtra backupJobExtra);
    }
}