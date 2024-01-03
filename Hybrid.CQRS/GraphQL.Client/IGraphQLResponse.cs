using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.CQRS.GraphQL.Client
{
    /// <summary>
    /// A type equivalent to a javascript map. Create a custom json converter for this class to customize your serializers behaviour
    /// </summary>
    public class Map : Dictionary<string, object> { }

    public interface IGraphQLResponse
    {
        object Data { get; }
        GraphQLError[]? Errors { get; set; }
        Map? Extensions { get; set; }
    }
}
