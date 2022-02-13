using System;
using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class StorageRPByFirstHybridCombination : IStorageRPByHybridCombination
    { // нужно удалить точку, если она не подходит хотя бы под один установленный лимит
        private DateTime _date;
        private int _quantity;

        public StorageRPByFirstHybridCombination(DateTime date, int quantity)
        {
            _date = date;
            _quantity = quantity;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();
            DateTime dateOfLastRestorePointCreation = backupJobExtra.GetBackupJob.RestorePoints[backupJobExtra.GetBackupJob.RestorePoints.Count - 1].DateOfCreation;
            if (dateOfLastRestorePointCreation < _date && _quantity == 0) return null; // Условие на то чтобы не удалить все ресторпоинты
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count; rp++)
            {
                if (backupJobExtra.GetBackupJob.RestorePoints[rp].DateOfCreation < _date)
                {
                    restorePointsToDelete.Add(backupJobExtra.GetBackupJob.RestorePoints[rp]);
                }
            }

            if (_quantity < backupJobExtra.GetBackupJob.RestorePoints.Count)
            {
                for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count - _quantity; rp++)
                {
                    restorePointsToDelete.Add(backupJobExtra.GetBackupJob.RestorePoints[rp]);
                }
            }

            return restorePointsToDelete;
        }
    }
}