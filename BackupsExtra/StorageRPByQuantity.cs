using System.Collections.Generic;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class StorageRPByQuantity : IDeleteRestorePoints
    { // validation done, log no
        private int _quantity;

        public StorageRPByQuantity(int quantity)
        {
            _quantity = quantity;
        }

        public void Delete(BackupJobExtra backupJobExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            if (_quantity == 0) return; // Чтобы не удалить все рп
            if (_quantity >= backupJobExtra.GetBackupJob.RestorePoints.Count) return;
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count - _quantity; rp++)
            {
                backupJobExtra.DeleteRestorePoints(backupJobExtra.GetBackupJob.RestorePoints[rp].Name);
            }
        }
    }
}