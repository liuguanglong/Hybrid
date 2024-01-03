using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hybrid.CQRS.GraphQL.Client;

namespace GraphQL.Client.Serializer
{
    public interface IGraphQLJsonSerializer
    {
        string SerializeToString(GraphQLRequest request);
        Task<GraphQLResponse> DeserializeFromUtf8StreamAsync(Stream stream, CancellationToken cancellationToken);
    }
}
