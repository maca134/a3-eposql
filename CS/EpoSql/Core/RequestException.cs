#region usings

using System;

#endregion

namespace EpoSql.Core
{
    public class RequestException : Exception
    {
        public RequestException()
        {
        }

        public RequestException(string message)
            : base(message)
        {
        }

        public RequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}