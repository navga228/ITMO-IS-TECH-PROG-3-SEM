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
            if (restorePoint == null)
            {
                throw new BackupsExtraException("RestorePoint is null!");
            }

            _restorePoint = restorePoint;
        }

        public RestorePoint GetRestorePoint
        {
            get => _restorePoint;
        }
    }
}