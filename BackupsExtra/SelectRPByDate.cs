using System;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class SelectRPByDate : IStorageRPMethod
    {
        private DateTime _date;

        public StorageRPByDate(DateTime dateToDelete)
        {
            if (dateToDelete == null)
            {
                throw new BackupsExtraException("dateToDelete is null!");
            }

            _date = dateToDelete;
        }

        public void Delete(BackupJobExtra backupJobExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            DateTime dateOfLastRestorePointCreation = backupJobExtra.GetBackupJob.RestorePoints[backupJobExtra.GetBackupJob.RestorePoints.Count - 1].DateOfCreation;
            if (dateOfLastRestorePointCreation < _date) return; // Чтобы не удалить все рп
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count; rp++)
            {
                if (backupJobExtra.GetBackupJob.RestorePoints[rp].DateOfCreation < _date)
                {
                    backupJobExtra.DeleteRestorePoints(backupJobExtra.GetBackupJob.RestorePoints[rp].Name);
                }
            }
        }
    }
}