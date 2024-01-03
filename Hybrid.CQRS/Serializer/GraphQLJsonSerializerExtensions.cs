using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Client.Serializer
{
    public static class GraphQLJsonSerializerExtensions
    {
        public static TOptions New<TOptions>(this Action<TOptions> configure) =>
            configure.AndReturn(Activator.CreateInstance<TOptions>());

        public static TOptions AndReturn<TOptions>(this Action<TOptions> configure, TOptions options)
        {
            configure(options);
            return options;
        }
    }
}
