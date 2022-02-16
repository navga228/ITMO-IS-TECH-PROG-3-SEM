using System;
using System.IO;
using System.Text;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class FileLog : ILog
    {
        private bool _includeTime;
        private string _fileLogPath;

        public FileLog(string path, bool includeTime)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new BackupsExtraException("Path is null or empty!");
            }

            _fileLogPath = path;
            _includeTime = includeTime;
        }

        public void Print(string message)
        {
            string finalLogMessage = string.Empty;
            if (_includeTime) finalLogMessage += "[" + DateTime.Now.ToString() + "] ";
            finalLogMessage += message + "\n";

            File.AppendAllText(_fileLogPath, finalLogMessage);
        }
    }
}