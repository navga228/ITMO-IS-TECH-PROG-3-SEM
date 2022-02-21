using System;
using System.Collections.Generic;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class SingleStorageAlgorithmExtra : IBackupAlgorithmExtra
    {
        private SingleStorageAlgorithm _singleStorageAlgorithm;

        public SingleStorageAlgorithmExtra(SingleStorageAlgorithm singleStorageAlgorithm)
        {
            _singleStorageAlgorithm = singleStorageAlgorithm ?? throw new BackupsExtraException("singleStorageAlgorithm is null!");
        }

        public RestorePoint CreateBackup(string restorePointName, string backupJobName, List<JobObject> jobObjects, IRepository repository)
        {
            if (string.IsNullOrEmpty(restorePointName))
            {
                throw new BackupsExtraException("restorePointName is null or empty!");
            }

            if (string.IsNullOrEmpty(backupJobName))
            {
                throw new BackupsExtraException("backupJobName is null or empty!");
            }

            if (jobObjects == null)
            {
                throw new BackupsExtraException("jobObjects is null!");
            }

            if (repository == null)
            {
                throw new BackupsExtraException("repository is null!");
            }

            return _singleStorageAlgorithm.CreateBackup(restorePointName, backupJobName, jobObjects, repository);
        }
    }
}