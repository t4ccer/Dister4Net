using System;
using System.Collections.Generic;
using System.Text;

namespace Dister4Net.Exceptions
{
    public class CommunicatorException : Exception
    {
        public CommunicatorException(string message) : base(message)
        {
        }

        public CommunicatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
