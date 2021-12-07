using System;
using System.Collections.Generic;
using System.IO;

namespace Backups
{
    public class SplitStoragesAlgorithm : IBackupAlgorithm
    {
        public RestorePoint CreateBackup(string restorePointName, string pathToDerictoryWithCompressedFiles, List<JobObject> jobObjects, IRepository repository)
        {
            repository.CompressFiles(jobObjects, pathToDerictoryWithCompressedFiles); // pathToDerictoryWithCompressedFiles формируется из _root + BackUpJob.Name + Restorepoint.Name
            return new RestorePoint(restorePointName, DateTime.Now, jobObjects);
        }
    }
}