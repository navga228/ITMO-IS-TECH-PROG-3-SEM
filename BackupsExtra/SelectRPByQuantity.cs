using System.Collections.Generic;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class SelectRPByQuantity : ISelectRPMethod
    {
        private int _quantity;

        public SelectRPByQuantity(int quantity)
        {
            _quantity = quantity;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();

            if (_quantity == 0) return null; // Чтобы не удалить все рп
            if (_quantity >= backupJobExtra.GetBackupJob.RestorePoints.Count) return restorePointsToDelete;
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count - _quantity; rp++)
            {
                restorePointsToDelete.Add(backupJobExtra.GetBackupJob.RestorePoints[rp]);
            }

            return restorePointsToDelete;
        }
    }
}