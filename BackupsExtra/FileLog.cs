using System;
using System.IO;
using System.Text;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    public class FileLog : ILog
    { // validation done but log no
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

        public void Print(LogLevel logLevel, string message)
        {
            string finalLogMessage = string.Empty;
            if (_includeTime) finalLogMessage += "[" + DateTime.Now.ToString() + "] ";
            Console.WriteLine(_includeTime ? "[" + DateTime.Now + "] " : string.Empty + message);
            switch (logLevel)
            {
                case LogLevel.Info:
                    finalLogMessage += "[INFO] ";
                    break;
                case LogLevel.Error:
                    finalLogMessage += "[ERROR] ";
                    break;
            }

            finalLogMessage += message + "\n";
            using (FileStream sourceStream = new FileStream(_fileLogPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(finalLogMessage);
                sourceStream.Write(info, 0, info.Length);
            }
        }
    }
}