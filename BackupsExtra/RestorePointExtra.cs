using System;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class RestorePointExtra
    {
        private RestorePoint _restorePoint;

        public RestorePointExtra(RestorePoint restorePoint)
        {
            _restorePoint = restorePoint ?? throw new BackupsExtraException("RestorePoint is null!");
        }

        public RestorePoint GetRestorePoint
        {
            get => _restorePoint;
        }
    }
}