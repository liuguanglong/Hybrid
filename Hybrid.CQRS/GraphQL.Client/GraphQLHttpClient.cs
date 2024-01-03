using GraphQL.Client.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.CQRS.GraphQL.Client
{
    public class GraphQLHttpClient : IGraphQLClient, IDisposable
    {

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly bool _disposeHttpClient = false;

        /// <summary>
        /// the json serializer
        /// </summary>
        public IGraphQLJsonSerializer JsonSerializer { get; }

        /// <summary>
        /// the instance of <see cref="HttpClient"/> which is used internally
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The Options	to be used
        /// </summary>
        public GraphQLHttpClientOptions Options { get; }

        #region Constructors

        public void SetToken(string token)
        {
            Options.Token = token;
        }

        public void ClearToken()
        {
            Options.Token = null;
        }

        public GraphQLHttpClient(string endPoint, string apiKey, IGraphQLJsonSerializer serializer)
            : this(new Uri(endPoint), apiKey, serializer) { }

        public GraphQLHttpClient(Uri endPoint, string apiKey, IGraphQLJsonSerializer serializer)
            : this(o => { o.EndPoint = endPoint; o.apiKey = apiKey; }, serializer) { }

        public GraphQLHttpClient(Action<GraphQLHttpClientOptions> configure, IGraphQLJsonSerializer serializer)
            : this(configure.New(), serializer) { }

        public GraphQLHttpClient(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
            : this(options, serializer, new HttpClient(options.HttpMessageHandler))
        {
            // set this flag to dispose the internally created HttpClient when GraphQLHttpClient gets disposed
            _disposeHttpClient = true;
        }

        public GraphQLHttpClient(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer, HttpClient httpClient)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            JsonSerializer = serializer ?? throw new ArgumentNullException(nameof(serializer), "please configure the JSON serializer you want to use");
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion

        #region IGraphQLClient

        /// <inheritdoc />
        public async Task<GraphQLHttpResponse> SendQueryAsync(GraphQLRequest request, CancellationToken cancellationToken = default)
        {
            return await SendHttpRequestAsync(request, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<GraphQLHttpResponse> SendMutationAsync(GraphQLRequest request,
            CancellationToken cancellationToken = default)
            => SendQueryAsync(request, cancellationToken);

        #endregion

        #region Private Methods

        private async Task<GraphQLHttpResponse> SendHttpRequestAsync(GraphQLRequest request, CancellationToken cancellationToken = default)
        {
            var preprocessedRequest = await Options.PreprocessRequest(request, this).ConfigureAwait(false);

            using var httpRequestMessage = preprocessedRequest.ToHttpRequestMessage(Options, JsonSerializer);
            using var httpResponseMessage = await HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

            if (Options.IsValidResponseToDeserialize(httpResponseMessage))
            {
                var graphQLResponse = await JsonSerializer.DeserializeFromUtf8StreamAsync(contentStream, cancellationToken).ConfigureAwait(false);
                return graphQLResponse.ToGraphQLHttpResponse(httpResponseMessage.Headers, httpResponseMessage.StatusCode);
            }

            // error handling
            string content = null;
            if (contentStream != null)
            {
                using var sr = new StreamReader(contentStream);
                content = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            throw new GraphQLHttpRequestException(httpResponseMessage.StatusCode, httpResponseMessage.Headers, content);
        }
        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged resources
        /// </summary>
        public void Dispose()
        {
            lock (_disposeLocker)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    Dispose(true);
                }
            }
        }

        private volatile bool _disposed;
        private readonly object _disposeLocker = new();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Debug.WriteLine($"Disposing GraphQLHttpClient on endpoint {Options.EndPoint}");
                _cancellationTokenSource.Cancel();
                if (_disposeHttpClient)
                    HttpClient.Dispose();
                _cancellationTokenSource.Dispose();
            }
        }

        #endregion
    }
}
