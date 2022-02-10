namespace BackupsExtra
{
    public enum LogLevel
    {
        /// <summary>обычные сообщения, информирующие о действиях системы</summary>
        Info,

        /// <summary>ошибка в работе системы</summary>
        Error,
    }

    public interface ILog
    {
        public void Print(LogLevel logLevel, string message);
    }
}