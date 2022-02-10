using System;

namespace BackupsExtra
{
    public class StorageRPByFirstHybridCombination : IStorageRPByHybrid, IDeleteRestorePoints
    { // нужно удалить точку, если она не подходит хотя бы под один установленный лимит
        private DateTime _date;
        private int _quantity;

        public StorageRPByFirstHybridCombination(DateTime date, int quantity)
        {
            _date = date;
            _quantity = quantity;
        }

        public void Delete(BackupJobExtra backupJobExtra)
        {
            DateTime dateOfLastRestorePointCreation = backupJobExtra.GetBackupJob.RestorePoints[backupJobExtra.GetBackupJob.RestorePoints.Count - 1].DateOfCreation;
            if (dateOfLastRestorePointCreation < _date && _quantity == 0) return; // Условие на то чтобы не удалить все ресторпоинты
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count; rp++)
            {
                if (backupJobExtra.GetBackupJob.RestorePoints[rp].DateOfCreation < _date)
                {
                    backupJobExtra.DeleteRestorePoints(backupJobExtra.GetBackupJob.RestorePoints[rp].Name);
                }
            }

            if (_quantity < backupJobExtra.GetBackupJob.RestorePoints.Count)
            {
                for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count - _quantity; rp++)
                {
                    backupJobExtra.DeleteRestorePoints(backupJobExtra.GetBackupJob.RestorePoints[rp].Name);
                }
            }
        }
    }
}