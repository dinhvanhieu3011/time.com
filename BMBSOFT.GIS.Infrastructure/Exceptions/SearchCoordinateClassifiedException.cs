using System;
using System.Collections.Generic;
using System.Text;

namespace BMBSOFT.GIS.Infrastructure.Exceptions
{
    public class SearchCoordinateException : Exception
    {
        public string OverrideMessage { get; }
        public SearchCoordinateException(string message) : base(message)
        {
            OverrideMessage = message;
        }
        public string Code { get; }

        public SearchCoordinateException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }
        public SearchCoordinateException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }
        public SearchCoordinateException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }
        public SearchCoordinateException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}