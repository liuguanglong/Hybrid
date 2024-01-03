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
    public class UIEvent
    {
        public String? EventName { get; set; }
        public String? SuccessMessage { get; set; }
        public String? ExceptionMessage { get; set; }
        public String? ConfirmMessage { get; set; }
        public BcParamString? NavigateURL { get; set; }
    }

    public class BcButton : UIElement
    {
        public UIEvent Event { get; set; }
        public String? Parent { get; set; }

        public BcButton(Dictionary<String,Object> data) : base(data)
        {
            String successMessage = data.getValue<String>("success_message");
            String errorMessage = data.getValue<String>("exception_message");
            String confirmMessage = data.getValue<String>("confirm_message");
            String url = data.getValue<String>("target_url");
            String eventName = data.getValue<String>("event_name");
            this.Parent = data.getValue<String>("parent");

            List<Dictionary<String, Object>> listParams = data.getList("paramlist");
            BcParam[] arrayParams = new BcParam[listParams.Count];
            foreach(var param in listParams)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayParams[index] = new BcParam(path,type);
            }

            Event = new UIEvent { EventName = eventName, SuccessMessage = successMessage, ExceptionMessage = errorMessage, NavigateURL = 
                new BcParamString(url, arrayParams), ConfirmMessage = confirmMessage };
        }
    }
}
    