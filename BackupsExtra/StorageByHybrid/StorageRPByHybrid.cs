namespace BackupsExtra
{
    public class StorageRPByHybrid : IDeleteRestorePoints
    {
        private IStorageRPByHybrid _storageRpByHybrid;

        public StorageRPByHybrid(IStorageRPByHybrid storageRpByHybrid)
        {
            _storageRpByHybrid = storageRpByHybrid;
        }

        public void Delete(BackupJobExtra backupJobExtra)
        {
            _storageRpByHybrid.Delete(backupJobExtra);
        }
    }
}