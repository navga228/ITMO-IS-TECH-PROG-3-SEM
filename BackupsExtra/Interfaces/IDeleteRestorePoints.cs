using System.Collections.Generic;

namespace BackupsExtra
{
    public interface IDeleteRestorePoints
    {
        public void Delete(BackupJobExtra backupJobExtra);
    }
}