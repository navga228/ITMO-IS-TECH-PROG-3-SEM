using System;

namespace BackupsExtra
{
    [Serializable]
    public class ConsoleLog : ILog
    {
        private bool _includeTime;

        public ConsoleLog(bool includeTime)
        {
            _includeTime = includeTime;
        }

        public void Print(string message)
        {
            string finalLogMessage = string.Empty;
            if (_includeTime) finalLogMessage += "[" + DateTime.Now.ToString() + "] ";
            finalLogMessage += message;
            Console.WriteLine(finalLogMessage);
        }
    }
}