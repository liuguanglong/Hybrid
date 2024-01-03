using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcSelect : UIElement
    {
        public UIDataFieldType DataType { get; set; }
        public bool SingleSelect { get; set; } = true;
        public String OptionDataPath { get; set; }
        public bool SelectKey { get; set; } = false;

        public BcSelect(String name, String label, String datapath, UIDataFieldType dataType, String optionDataPath) : base(name, label, datapath)
        {
            DataType = dataType;
            OptionDataPath = optionDataPath;
        }

        public BcSelect(String name, String label, String datapath, UIDataFieldType dataType, String optionDataPath, bool bSignleSelect) : base(name, label, datapath)
        {
            DataType = dataType;
            this.SingleSelect = bSignleSelect;
            OptionDataPath = optionDataPath;
        }

        public BcSelect(Dictionary<String, Object> data) : base(data)
        {
            SingleSelect = data.getValue<bool>("SingleSelect");
            OptionDataPath = data.getValue<String>("option_datapath");
            SelectKey = data.getValue<bool>("SelectKey");

            String t = data.getValue<String>("data_type");
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
                case "guid":
                    this.DataType = UIDataFieldType.Guid; break;
                default:
                    this.DataType = UIDataFieldType.String; break;
            }
        }
    }
}
