using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcField : UIElement
    {
        public UIDataFieldType DataType { get; set; }
        public int Lines { get; set; } = 1;

        public BcField(String name, String label, String datapath, UIDataFieldType dataType) : base(name, label, datapath)
        {
            DataType = dataType;
        }

        public BcField(Dictionary<String,Object> data) : base(data) {

            String t = data.getValue<String>("data_type");
            this.Lines = data.getValue<Int32>("lines");

            switch (t.ToLower())
            {
                case "string":
                    this.DataType = UIDataFieldType.String; break;
                case "int32":
                    this.DataType = UIDataFieldType.Int32; break;
                case "int64":
                    this.DataType = UIDataFieldType.Int64; break;
                case "double":
                    this.DataType = UIDataFieldType.Double; break;
                case "datetime":
                    this.DataType = UIDataFieldType.DateTime; break;
                case "bool":
                    this.DataType= UIDataFieldType.Boolean; break;
                case "guid":
                    this.DataType = UIDataFieldType.Guid; break;
                default:
                    this.DataType = UIDataFieldType.String; break;
            }
        }
    }
}
