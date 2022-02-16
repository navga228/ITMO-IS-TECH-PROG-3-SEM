using System;
using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    [Serializable]
    public class SelectRPByHybrid : ISelectRPMethod
    {
        private ISelectRPByHybridCombination _hybridCombination;

        public SelectRPByHybrid(ISelectRPByHybridCombination selectRpByHybrid)
        {
            _hybridCombination = selectRpByHybrid;
        }

        public void SetHybridCombination(ISelectRPByHybridCombination hybridCombination)
        {
            _hybridCombination = hybridCombination;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            return _hybridCombination.Select(backupJobExtra);
        }
    }
}