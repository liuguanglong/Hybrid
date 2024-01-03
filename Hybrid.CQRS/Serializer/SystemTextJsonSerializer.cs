using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Hybrid.CQRS.GraphQL.Client;

namespace GraphQL.Client.Serializer
{
    public class SystemTextJsonSerializer : IGraphQLJsonSerializer
    {
        public static JsonSerializerOptions DefaultJsonSerializerSettings => new()
        {
             PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
             Converters = { new JsonStringEnumConverter(), new BussinessCoreJsonConverter() }
        };

        public JsonSerializerOptions JsonSerializerSettings { get; }

        public SystemTextJsonSerializer() : this(DefaultJsonSerializerSettings) { }

        public SystemTextJsonSerializer(Action<JsonSerializerOptions> configure) : this(configure.AndReturn(DefaultJsonSerializerSettings)) { }

        public SystemTextJsonSerializer(JsonSerializerOptions jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
            ConfigureMandatorySerializerOptions();
        }

        // deserialize extensions to Dictionary<string, object>
        private void ConfigureMandatorySerializerOptions() => JsonSerializerSettings.Converters.Insert(0, new BussinessCoreJsonConverter());

        public string SerializeToString(GraphQLRequest request) => JsonSerializer.Serialize(request, JsonSerializerSettings);

        public Task<GraphQLResponse> DeserializeFromUtf8StreamAsync(Stream stream, CancellationToken cancellationToken) =>
            DeserializeFromUtf8Stream(stream);

        private Task<GraphQLResponse> DeserializeFromUtf8Stream(Stream stream)
        {
            return Task.FromResult(JsonSerializer.Deserialize<GraphQLResponse>(stream, JsonSerializerSettings));
        }
    }
}
