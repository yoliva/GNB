using System;

namespace GNB.Infrastructure.Capabilities
{
    public class GNBException : Exception
    {
        public GNBException() { }

        public GNBException(string message, ErrorCode code) : base(message)
        {
            Code = (int)code;
        }

        public GNBException(string message, ErrorCode code, Exception innerException) : base(message, innerException)
        {
            Code = (int)code;
        }

        public int Code { get; set; }
    }
}
