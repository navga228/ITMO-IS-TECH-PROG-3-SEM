using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class SelectRPByHybrid : ISelectRPMethod
    {
        private IStorageRPByHybridCombination _hybridCombination;

        public SelectRPByHybrid(IStorageRPByHybridCombination storageRpByHybrid)
        {
            _hybridCombination = storageRpByHybrid;
        }

        public void SetHybridCombination(IStorageRPByHybridCombination hybridCombination)
        {
            _hybridCombination = hybridCombination;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            return _hybridCombination.Select(backupJobExtra);
        }
    }
}