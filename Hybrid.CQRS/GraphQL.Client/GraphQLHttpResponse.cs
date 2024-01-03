using System.Net.Http.Headers;
using System.Net;

namespace Hybrid.CQRS.GraphQL.Client;
public class GraphQLHttpResponse : GraphQLResponse
{
    public GraphQLHttpResponse(GraphQLResponse response, HttpResponseHeaders responseHeaders, HttpStatusCode statusCode)
    {
        Data = response.Data;
        Errors = response.Errors;
        Extensions = response.Extensions;
        ResponseHeaders = responseHeaders;
        StatusCode = statusCode;
    }

    public HttpResponseHeaders ResponseHeaders { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}

public static class GraphQLResponseExtensions
{
    public static GraphQLHttpResponse ToGraphQLHttpResponse(this GraphQLResponse response, HttpResponseHeaders responseHeaders, HttpStatusCode statusCode) => new(response, responseHeaders, statusCode);

    /// <summary>
    /// Casts <paramref name="response"/> to <see cref="GraphQLHttpResponse{T}"/>. Throws if the cast fails.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <exception cref="InvalidCastException"><paramref name="response"/> is not a <see cref="GraphQLHttpResponse{T}"/></exception>
    /// <returns></returns>
    public static GraphQLHttpResponse AsGraphQLHttpResponse(this GraphQLResponse response) => (GraphQLHttpResponse)response;
}
