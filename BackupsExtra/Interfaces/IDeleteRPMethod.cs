using System.Collections.Generic;
using Backups;

namespace BackupsExtra
{
    public interface IDeleteRPMethod
    {
        public void Delete(List<RestorePoint> rpForDelete, BackupJobExtra backupJobExtra);
    }
}