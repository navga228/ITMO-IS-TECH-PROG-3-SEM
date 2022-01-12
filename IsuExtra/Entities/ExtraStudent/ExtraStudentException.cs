using System;

namespace IsuExtra.Entities.ExtraStudent
{
    public class ExtraStudentException : Exception
    {
        public ExtraStudentException()
        {
        }

        public ExtraStudentException(string message)
            : base(message)
        {
        }

        public ExtraStudentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}