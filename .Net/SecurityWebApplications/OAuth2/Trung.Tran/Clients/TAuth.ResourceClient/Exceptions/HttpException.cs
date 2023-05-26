using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace TAuth.ResourceClient.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException()
        {
        }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HttpResponseMessage Response { get; set; }
    }
}
