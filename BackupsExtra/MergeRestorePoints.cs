using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class MergeRestorePoints : IDeleteRPMethod
    {
        private IRepositoryExtra _repositoryExtra;

        public MergeRestorePoints(IRepositoryExtra localFilesRepositoryExtra)
        {
            _repositoryExtra = localFilesRepositoryExtra ?? throw new BackupsExtraException("localFilesRepositoryExtra is null!");
        }

        public void Delete(List<RestorePoint> rpForDelete, BackupJobExtra backupJobExtra)
        { // По итогу наш список для удаления схлопнется и все смерджится в самую старую точку, которая останется в цепочке рестор поинтов дальше.
            if (rpForDelete == null)
            {
                throw new BackupsExtraException("rpForDelete is null!");
            }

            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            var lastRP = backupJobExtra.GetBackupJob.RestorePoints[rpForDelete.Count]; // Точка в которую будут вмердживаться все точки из списка
            if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithm)
            {
                foreach (var rp in rpForDelete)
                {
                    backupJobExtra.DeleteRestorePoint(rp.Name);
                }
            }
            else
            {
                foreach (var rp in rpForDelete)
                {
                    foreach (var jobObject in rp.BachupedFiles.Where(jobObject => !lastRP.BachupedFiles.Contains(jobObject)))
                    {
                        lastRP.BachupedFiles.Add(jobObject);
                        _repositoryExtra.CopyFile(
                            _repositoryExtra.GetRoot() + backupJobExtra.GetBackupJob.Name + "/" + rp.Name + "/" + jobObject.Name + ".zip",
                            _repositoryExtra.GetRoot() + backupJobExtra.GetBackupJob.Name + "/" + lastRP.Name + "/" + jobObject.Name + ".zip");
                    }

                    backupJobExtra.DeleteRestorePoint(rp.Name);
                }
            }
        }
    }
}