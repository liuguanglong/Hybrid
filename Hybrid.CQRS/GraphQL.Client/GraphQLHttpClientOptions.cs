using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Net;

namespace Hybrid.CQRS.GraphQL.Client
{
    /// <summary>
    /// The Options that the <see cref="GraphQLHttpClient"/> will use.
    /// </summary>
    public class GraphQLHttpClientOptions
    {
        /// <summary>
        /// The GraphQL EndPoint to be used
        /// </summary>
        public Uri? EndPoint { get; set; }

        /// <summary>
        /// The <see cref="System.Net.Http.HttpMessageHandler"/> that is going to be used
        /// </summary>
        public HttpMessageHandler HttpMessageHandler { get; set; } = new HttpClientHandler();

        /// <summary>
        /// The <see cref="MediaTypeHeaderValue"/> that will be send on POST
        /// </summary>
        public string MediaType { get; set; } = "application/json"; // This should be "application/graphql" also "application/x-www-form-urlencoded" is Accepted

        /// <summary>
        /// Request preprocessing function. Can be used i.e. to inject authorization info into a GraphQL request payload.
        /// </summary>
        public Func<GraphQLRequest, GraphQLHttpClient, Task<GraphQLHttpRequest>> PreprocessRequest { get; set; } =
            (request, client) =>
                Task.FromResult(request is GraphQLHttpRequest graphQLHttpRequest ? graphQLHttpRequest : new GraphQLHttpRequest(request));

        /// <summary>
        /// Delegate to determine if GraphQL response may be properly deserialized into <see cref="GraphQLResponse{T}"/>.
        /// Note that compatible to the draft graphql-over-http spec GraphQL Server MAY return 4xx status codes (401/403, etc.)
        /// with well-formed GraphQL response containing errors collection.
        /// </summary>
        public Func<HttpResponseMessage, bool> IsValidResponseToDeserialize { get; set; } = DefaultIsValidResponseToDeserialize;

        private static readonly IReadOnlyCollection<string> _acceptedResponseContentTypes = new[] { "application/graphql+json", "application/json", "application/graphql-response+json" };

        public static bool DefaultIsValidResponseToDeserialize(HttpResponseMessage r)
        {
            if (r.Content.Headers.ContentType?.MediaType != null && !_acceptedResponseContentTypes.Contains(r.Content.Headers.ContentType.MediaType))
                return false;

            return r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// The default user agent request header.
        /// Default to the GraphQL client assembly.
        /// </summary>
        public ProductInfoHeaderValue? DefaultUserAgentRequestHeader { get; set; }
            = new ProductInfoHeaderValue(typeof(GraphQLHttpClient).Assembly.GetName().Name, typeof(GraphQLHttpClient).Assembly.GetName().Version.ToString());
        public string apiKey { get; internal set; }
        public string? Token { get; set; }
    }
}