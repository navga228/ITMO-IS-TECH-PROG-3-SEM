using System;

namespace BackupsExtra
{
    public class ConsoleLog : ILog
    {
        private bool _includeTime;

        public ConsoleLog(bool includeTime)
        {
            _includeTime = includeTime;
        }

        public void Print(LogLevel logLevel, string message)
        {
            string finalLogMessage = string.Empty;
            if (_includeTime) finalLogMessage += "[" + DateTime.Now.ToString() + "] ";
            switch (logLevel)
            {
                case LogLevel.Info:
                    finalLogMessage += "[INFO] ";
                    break;
                case LogLevel.Error:
                    finalLogMessage += "[ERROR] ";
                    break;
            }

            finalLogMessage += message;
            Console.WriteLine(finalLogMessage);
        }
    }
}