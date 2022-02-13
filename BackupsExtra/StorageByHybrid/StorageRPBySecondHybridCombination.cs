using System;
using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public class StorageRPBySecondHybridCombination : IStorageRPByHybridCombination
    { // нужно удалить точку, если она не подходит за все установленные лимиты
        private DateTime _date;
        private int _quantity;

        public StorageRPBySecondHybridCombination(DateTime date, int quantity)
        {
            _date = date;
            _quantity = quantity;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();
            foreach (var rp in backupJobExtra.GetBackupJob.RestorePoints)
            {
                int index = backupJobExtra.GetBackupJob.RestorePoints.IndexOf(rp);
                if (rp.DateOfCreation < _date && index + 1 < _quantity)
                { // Если дата создания рп меньше установленой даты
                  // и если рп находится в цеопчке по своему номеру + 1 ниже чем разрешенное кол-во рп
                  restorePointsToDelete.Add(rp);
                }
            }

            return restorePointsToDelete;
        }
    }
}