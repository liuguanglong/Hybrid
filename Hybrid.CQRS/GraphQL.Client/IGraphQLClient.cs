using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.CQRS.GraphQL.Client
{
    public interface IGraphQLClient
    {
        Task<GraphQLHttpResponse> SendQueryAsync(GraphQLRequest request, CancellationToken cancellationToken = default);
        Task<GraphQLHttpResponse> SendMutationAsync(GraphQLRequest request, CancellationToken cancellationToken = default);
    }
}
