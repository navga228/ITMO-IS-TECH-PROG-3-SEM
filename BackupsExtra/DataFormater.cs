using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class DataFormater
    {
        private IRepositoryExtra _repository;
        public DataFormater(IRepositoryExtra repositoryExtra)
        {
            _repository = repositoryExtra;
        }

        public void SaveData(BackupJobExtra backupJobExtra)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("BackupJobExtra is null!");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            // получаем поток, куда будем записывать сериализованный объект
            using (var fs = _repository.OpenWriteStream(_repository.GetRoot() + backupJobExtra.GetBackupJob.Name + "/" + "BackupJobExtraData" + ".dat"))
            {
                if (fs != Stream.Null) formatter.Serialize(fs, backupJobExtra);
            }
        }

        public BackupJobExtra GetData(string backupJobExtraName)
        {
            if (string.IsNullOrEmpty(backupJobExtraName))
            {
                throw new BackupsExtraException("backupJobExtraName is null or empty!");
            }

            if (_repository.SearchFile(_repository.GetRoot() + backupJobExtraName + "/" + "BackupJobExtraData" + ".dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (var fs = _repository.OpenReadStrem(_repository.GetRoot() + backupJobExtraName + "/" + "BackupJobExtraData" + ".dat"))
                {
                    if (fs != Stream.Null) return (BackupJobExtra)formatter.Deserialize(fs);
                }
            }

            return null;
        }
    }
}