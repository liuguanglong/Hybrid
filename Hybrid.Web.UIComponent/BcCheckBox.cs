using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Web.UIComponent
{
    public class BcCheckBox : UIElement
    {
        public UIDataFieldType DataType { get; set; }
        public BcCheckBox(String name, String label, String datapath) : base(name, label, datapath)
        {
            DataType = UIDataFieldType.Boolean;
        }

        public BcCheckBox(Dictionary<String, Object> data) : base(data)
        {
            DataType = UIDataFieldType.Boolean;
        }
    }
}
