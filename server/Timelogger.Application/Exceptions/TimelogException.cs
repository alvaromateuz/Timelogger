using System;

namespace Timelogger.Application.Exceptions
{
    public class TimelogException : Exception
    {
        public TimelogException(string message) : base(message)
        {
        }
    }
}
