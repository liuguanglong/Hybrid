﻿using GraphQL.Client.Serializer;
using System.Net.Http.Headers;
using System.Text;

namespace Hybrid.CQRS.GraphQL.Client
{
    public class GraphQLHttpRequest : GraphQLRequest
    {
        public GraphQLHttpRequest()
        {
        }

        public GraphQLHttpRequest(string query, object? variables = null, string? operationName = null, Dictionary<string, object?>? extensions = null)
            : base(query, variables, operationName, extensions)
        {
        }

        public GraphQLHttpRequest(GraphQLRequest other)
            : base(other)
        {
        }

        /// <summary>
        /// Creates a <see cref="HttpRequestMessage"/> from this <see cref="GraphQLHttpRequest"/>.
        /// Used by <see cref="GraphQLHttpClient"/> to convert GraphQL requests when sending them as regular HTTP requests.
        /// </summary>
        /// <param name="options">the <see cref="GraphQLHttpClientOptions"/> passed from <see cref="GraphQLHttpClient"/></param>
        /// <param name="serializer">the <see cref="IGraphQLJsonSerializer"/> passed from <see cref="GraphQLHttpClient"/></param>
        /// <returns></returns>
        public virtual HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, options.EndPoint)
            {
                Content = new StringContent(serializer.SerializeToString(this), Encoding.UTF8, options.MediaType)
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/graphql-response+json"));
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            message.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            message.Headers.Add("apiKey", options.apiKey);

            if (options.Token != null)
            {
                message.Headers.Add("Authorization", options.Token);
            }

            if (options.DefaultUserAgentRequestHeader != null)
                message.Headers.UserAgent.Add(options.DefaultUserAgentRequestHeader);

            return message;
        }
    }
}