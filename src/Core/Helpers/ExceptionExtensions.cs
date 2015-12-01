using System;

namespace QOAM.Core.Helpers
{
    public static class ExceptionExtensions
    {
        public static string InnermostMessage(this Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception.Message;
        }
    }
}