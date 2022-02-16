using System;
using System.Collections.Generic;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class SelectRPByDate : ISelectRPMethod
    {
        private DateTime _date;

        public SelectRPByDate(DateTime dateToDelete)
        {
            if (dateToDelete == null)
            {
                throw new BackupsExtraException("dateToDelete is null!");
            }

            _date = dateToDelete;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            List<RestorePoint> restorePointsToDelete = new List<RestorePoint>();

            DateTime dateOfLastRestorePointCreation = backupJobExtra.GetBackupJob.RestorePoints[backupJobExtra.GetBackupJob.RestorePoints.Count - 1].DateOfCreation;
            if (dateOfLastRestorePointCreation < _date) return null; // Чтобы не удалить все рп
            for (int rp = 0; rp < backupJobExtra.GetBackupJob.RestorePoints.Count; rp++)
            {
                if (backupJobExtra.GetBackupJob.RestorePoints[rp].DateOfCreation < _date)
                {
                    restorePointsToDelete.Add(backupJobExtra.GetBackupJob.RestorePoints[rp]);
                }
            }

            return restorePointsToDelete;
        }
    }
}