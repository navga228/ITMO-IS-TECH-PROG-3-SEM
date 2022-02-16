using System;
using System.Collections.Generic;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class DeleteRestorPoints : IDeleteRPMethod
    {
        public void Delete(List<RestorePoint> rpForDelete, BackupJobExtra backupJobExtra)
        {
            if (rpForDelete == null)
            {
                throw new BackupsExtraException("rpForDelete is null");
            }

            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null");
            }

            foreach (var rp in rpForDelete)
            {
                backupJobExtra.DeleteRestorePoint(rp.Name);
            }
        }
    }
}