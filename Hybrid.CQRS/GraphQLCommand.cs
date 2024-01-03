using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hybrid.CQRS
{
    public class GraphQLCommand
    {
        [JsonIgnore]
        public String id { get; set; }
        public String name { get; set; }
        public String content { get; set; }
        public String schema { get; set; }
        public String paramSample { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set;} = DateTime.Now;
    }
}
