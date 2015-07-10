#region usings

using System;

#endregion

namespace EpoSql.Core
{
    public class ArmaArrayException : Exception
    {
        public ArmaArrayException()
        {
        }

        public ArmaArrayException(string message)
            : base(message)
        {
        }

        public ArmaArrayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}