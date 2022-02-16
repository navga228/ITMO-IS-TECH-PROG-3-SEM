using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class MergeRestorePoints : IDeleteRPMethod
    {
        private LocalFilesRepositoryExtra _repositoryExtra;

        public MergeRestorePoints(LocalFilesRepositoryExtra localFilesRepositoryExtra)
        {
            if (localFilesRepositoryExtra == null)
            {
                throw new BackupsExtraException("localFilesRepositoryExtra is null!");
            }

            _repositoryExtra = localFilesRepositoryExtra;
        }

        public void Delete(List<RestorePoint> rpForDelete, BackupJobExtra backupJobExtra)
        { // Мерджим точки паровозиком сначала мерджим все точки из списка(точек на удаление) до последней и последнюю мерджим с самой старой точкой цепочке рп, которая не будет удаляться.
            // По итогу наш список для удаления схлопнется и все смерджится в самую старую точку, которая останется в цепочке рестор поинтов дальше.
            if (rpForDelete == null)
            {
                throw new BackupsExtraException("rpForDelete is null!");
            }

            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("backupJobExtra is null!");
            }

            var lastRP = backupJobExtra.GetBackupJob.RestorePoints[rpForDelete.Count + 1]; // Точка в которую будут вмердживаться все точки из списка
            for (int rp1 = 0; rp1 < rpForDelete.Count - 1; rp1++)
            {
                for (int rp2 = 0; rp2 < rpForDelete.Count; rp2++)
                {
                    if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithm)
                    {
                        backupJobExtra.DeleteRestorePoint(rpForDelete[rp1].Name);
                    }
                    else
                    {
                        foreach (JobObject jobObject in rpForDelete[rp1].BachupedFiles.Where(jobObject => !rpForDelete[rp2].BachupedFiles.Contains(jobObject)))
                        {
                            rpForDelete[rp2].BachupedFiles.Add(jobObject);
                            _repositoryExtra.CopyFile(
                                backupJobExtra.GetBackupJob.Name + "/" + rpForDelete[rp1].Name + "/" + jobObject.Name + ".zip",
                                backupJobExtra.GetBackupJob.Name + "/" + rpForDelete[rp2].Name + "/" + jobObject.Name + ".zip");
                        }

                        backupJobExtra.DeleteRestorePoint(rpForDelete[rp1].Name);
                    }

                    if (backupJobExtra.GetBackupJob.RestorePoints.Count == backupJobExtra.GetBackupJob.RestorePoints.Count - rpForDelete.Count - 1)
                    { // если осталась действительно только одна точка из списка на удаление, то мерджим эту точку с lastRP
                        if (backupJobExtra.GetBackupJob.BackupAlgorithm is SingleStorageAlgorithm)
                        {
                            backupJobExtra.DeleteRestorePoint(rpForDelete[rpForDelete.Count - 1].Name);
                        }
                        else
                        {
                            foreach (JobObject jobObject in rpForDelete[rpForDelete.Count - 1].BachupedFiles.Where(jobObject => !lastRP.BachupedFiles.Contains(jobObject)))
                            {
                                lastRP.BachupedFiles.Add(jobObject);
                                _repositoryExtra.CopyFile(
                                    backupJobExtra.GetBackupJob.Name + "/" + rpForDelete[rpForDelete.Count - 1].Name + "/" + jobObject.Name + ".zip",
                                    backupJobExtra.GetBackupJob.Name + "/" + lastRP.Name + "/" + jobObject.Name + ".zip");
                            }

                            backupJobExtra.DeleteRestorePoint(rpForDelete[rpForDelete.Count - 1].Name);
                        }
                    }
                }
            }
        }
    }
}