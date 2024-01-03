using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hybrid.CQRS.GraphQL.Client
{
    public class GraphQLResponse : IGraphQLResponse, IEquatable<GraphQLResponse?>
    {
        [DataMember(Name = "data")]
        public JsonObject Data { get; set; }
        object IGraphQLResponse.Data => Data;

        [DataMember(Name = "errors")]
        public GraphQLError[]? Errors { get; set; }

        [DataMember(Name = "extensions")]
        public Map? Extensions { get; set; }

        public override bool Equals(object? obj) => Equals(obj as GraphQLResponse);

        public bool Equals(GraphQLResponse? other)
        {
            if (other == null)
            { return false; }
            if (ReferenceEquals(this, other))
            { return true; }
            if (!Data.Equals(other.Data))
            { return false; }

            if (Errors != null && other.Errors != null)
            {
                if (!Errors.SequenceEqual(other.Errors))
                { return false; }
            }
            else if (Errors != null && other.Errors == null)
            { return false; }
            else if (Errors == null && other.Errors != null)
            { return false; }

            if (Extensions != null && other.Extensions != null)
            {
                if (!Extensions.SequenceEqual(other.Extensions))
                { return false; }
            }
            else if (Extensions != null && other.Extensions == null)
            { return false; }
            else if (Extensions == null && other.Extensions != null)
            { return false; }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Data.GetHashCode();
                {
                    if (Errors != null)
                    {
                        foreach (var element in Errors)
                        {
                            hashCode = hashCode * 397 ^ EqualityComparer<GraphQLError?>.Default.GetHashCode(element);
                        }
                    }
                    else
                    {
                        hashCode = hashCode * 397 ^ 0;
                    }

                    if (Extensions != null)
                    {
                        foreach (var element in Extensions)
                        {
                            hashCode = hashCode * 397 ^ EqualityComparer<KeyValuePair<string, object>>.Default.GetHashCode(element);
                        }
                    }
                    else
                    {
                        hashCode = hashCode * 397 ^ 0;
                    }
                }
                return hashCode;
            }
        }

        public static bool operator ==(GraphQLResponse? response1, GraphQLResponse? response2) => EqualityComparer<GraphQLResponse?>.Default.Equals(response1, response2);

        public static bool operator !=(GraphQLResponse? response1, GraphQLResponse? response2) => !(response1 == response2);
    }
}