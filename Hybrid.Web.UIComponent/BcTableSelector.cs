using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{

    public class FieldFilter
    {
        public String FieldName { get; set; }
        public String DataPath { get; set; }
    }

    public class BcTableSelector
    {
        public String DataSource { get; set; }
        public String[] Fields { get; set; } = new String[0];
        public String[] TableColumns { get; set; } = new string[0];
        public String[] TableFilters { get; set; } = new string[0];        
        public List<FieldFilter> DataFilters { get; set; }= new List<FieldFilter>();

        public String BuildSearchString(Dictionary<String,Object> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DataSource).Append("?");
            foreach(var filter in DataFilters)
            {
                sb.Append($"{filter.FieldName}=eq.{data.getValue<string>(filter.DataPath)}").Append("&");
            }

            sb.Append("select=");
            foreach(var f in Fields)
            {
                sb.Append(f).Append(",");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
