using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcInput: UIElement
    {
        public UIDataFieldType DataType { get; set; }
        public int Lines { get; set; } = 1;

        public BcInput(String name,String label, String datapath, UIDataFieldType dataType) : base(name, label, datapath)
        {
            DataType = dataType;
        }

        public BcInput(Dictionary<String,Object> data) : base(data)
        {
            this.Lines = data.getValue<Int32>("lines");
            String t = data.getValue<String>("data_type");
            switch (t.ToLower())
            {
                case "string":
                    this.DataType= UIDataFieldType.String; break;
                case "int32":
                    this.DataType= UIDataFieldType.Int32; break;
                case "int64":
                    this.DataType = UIDataFieldType.Int64; break;
                case "double":
                    this.DataType = UIDataFieldType.Double; break;
                case "datetime":
                    this.DataType = UIDataFieldType.DateTime; break;
                case "guid":
                    this.DataType = UIDataFieldType.Guid; break;
                default:
                    this.DataType = UIDataFieldType.String; break;
            }
        }
    }
}
