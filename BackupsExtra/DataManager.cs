using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class DataManager
    {
        public void SaveData(BackupJobExtra backupJobExtra, string savePath)
        {
            if (backupJobExtra == null)
            {
                throw new BackupsExtraException("BackupJobExtra is null!");
            }

            if (string.IsNullOrEmpty(savePath))
            {
                throw new BackupsExtraException("Save path is null or empty!");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(fs, savePath);
            }
        }

        public BackupJobExtra GetData(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return (BackupJobExtra)formatter.Deserialize(fs);
            }
        }
    }
}