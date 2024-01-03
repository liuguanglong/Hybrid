using GraphQL.Client;
using GraphQL.Client.Serializer;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using Hybrid.CQRS.GraphQL.Client;

namespace Hybrid.CQRS
{
    public class CommandService
    {
        private String apiKey;
        private String baseurl;
        private String? token = null;

        private GraphQLHttpClient client;

        public CommandService(String url, String restfulBaseUrl, String apikey)
        {
            client = new GraphQLHttpClient(url, apikey, new SystemTextJsonSerializer());
            this.baseurl = restfulBaseUrl;
            this.apiKey = apikey;
        }

        public async Task SetToken(String token)
        {
            client.SetToken(token);
            this.token = token;
        }

        public async Task clearToken()
        {
            client.ClearToken();
            this.token = null;
        }

        public async Task<int> deleteCommandByName(String name)
        {
            Dictionary<String, Object> filter = new Dictionary<string, object>();
            filter.Add("name", name);

            var request = new GraphQLRequest
            {
                Query = @"
			   mutation deleteCommand($name:String!) {
                  deleteFromGraphQLCommandsCollection(filter: {name: {eq: $name}}) {
                    affectedCount
                    records {
                      id
                      name
                    }
                  }
                }",
                Variables = filter
            };

            GraphQLHttpResponse ret =  await client.SendQueryAsync(request);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (ret.Errors == null)
                    return (int)ret.Data["deleteFromGraphQLCommandsCollection"]["affectedCount"];
            }
            return 0;
        }

        public  async Task<JsonObject> addCommand(String name, String graphQL, String schema, String sample)
        {
            GraphQLCommand cmd = new GraphQLCommand();
            cmd.schema = schema;
            cmd.name = name;
            cmd.content = graphQL;
            cmd.paramSample = sample;

            List<GraphQLCommand> list = new List<GraphQLCommand>();
            list.Add(cmd);

            Dictionary<String, Object> commands = new Dictionary<string, object>();
            commands.Add("commands", list);

            var request = new GraphQLRequest
            {
                Query = @"
			   mutation insertCommand($commands:[GraphQLCommandsInsertInput!]!) {
                  insertIntoGraphQLCommandsCollection(objects: $commands) {
                    affectedCount
                    records {
                      id
                      name
                    }
                  }
                }",
                Variables = commands
            };

            GraphQLHttpResponse ret =  await client.SendQueryAsync(request);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (ret.Errors == null)
                {
                    return ret.Data;
                }
            }
            return null;
        }

        public async Task<JsonObject> query(String name, Dictionary<String, Object> param)
        {
            var cmd = await getCommandByName(name);
            if (cmd == null)
            {
                return null;
            }

            var request = new GraphQLRequest
            {
                Query = cmd.content,
                Variables = param
            };
            GraphQLHttpResponse ret = await client.SendQueryAsync(request);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (ret.Errors == null)
                {
                    return ret.Data;
                }
            }
            return null;
        }

        public async Task<JsonObject> fireDomainEvent(String eventName, Dictionary<String, Object> param)
        {
            var client = new HttpClient();
            String url = baseurl + eventName; 
            var request = new HttpRequestMessage(
                HttpMethod.Post, url);
            request.Headers.Add("apikey", this.apiKey);
            if(this.token != null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            Dictionary<String, Dictionary<String, Object>> data = new Dictionary<string, Dictionary<string, object>>();
            data.Add("data", param);

            String requestBody = JsonSerializer.Serialize(data); ;
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            String result = await response.Content.ReadAsStringAsync();
            return JsonObject.Parse(result) as JsonObject;
        }

        public async Task<GraphQLCommand> getCommandByName(String name)
        {
            Dictionary<String, Object> queryParams = new Dictionary<string, object>();
            queryParams.Add("name", name);

            String queryCommand = @"
			   query queryCommand($name:String!){
                  graphQLCommandsCollection(filter: {name: {eq: $name}}) {
                    edges {
                      node {
                        id
                        name
                        content
                        schema
                        paramSample
                        created_at
                        updated_at
                      }
                    }
                  }
                }";

            var request = new GraphQLRequest
            {
                Query = queryCommand,
                Variables = queryParams
            };

            GraphQLHttpResponse ret = await client.SendQueryAsync(request);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (ret.Errors == null)
                {
                    JsonArray array = (JsonArray)ret.Data["graphQLCommandsCollection"]["edges"];
                    if (array.Count == 0)
                        return null;

                    JsonObject data = (JsonObject)ret.Data["graphQLCommandsCollection"]["edges"][0]["node"];
                    var command = JsonSerializer.Deserialize<GraphQLCommand>(data.ToString());
                    return command;
                }
                return null;
            }
            return null;
        }
    }
}
