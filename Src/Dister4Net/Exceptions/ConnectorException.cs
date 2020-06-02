using System;
using System.Collections.Generic;
using System.Text;

namespace Dister4Net.Exceptions
{
    public class ConnectorException : Exception
    {
        public ConnectorException(string message) : base(message)
        {
        }

        public ConnectorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
