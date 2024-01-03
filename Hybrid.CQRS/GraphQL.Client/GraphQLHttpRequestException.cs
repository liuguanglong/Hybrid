using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace Hybrid.CQRS.GraphQL.Client
{
    [Serializable]
    internal class GraphQLHttpRequestException : Exception
    {
        private HttpStatusCode statusCode;
        private HttpResponseHeaders headers;
        private string? content;

        public GraphQLHttpRequestException()
        {
        }

        public GraphQLHttpRequestException(string? message) : base(message)
        {
        }

        public GraphQLHttpRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public GraphQLHttpRequestException(HttpStatusCode statusCode, HttpResponseHeaders headers, string? content)
        {
            this.statusCode = statusCode;
            this.headers = headers;
            this.content = content;
        }

        protected GraphQLHttpRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}