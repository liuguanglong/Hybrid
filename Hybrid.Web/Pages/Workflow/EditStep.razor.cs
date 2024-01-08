using Hybrid.CQRS;
using Hybrid.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.Pages.Workflow
{
    public partial class EditStep
    {
        [Parameter]
        public Dictionary<String, Object> data { get; set; }
        public List<(String,String)> stepTypes { get; set; }

        [Inject]
        public CommandService serviceCommand { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            stepTypes = new List<(string, string)>
            {
                ("Event","Event"),
                ("While","While"),
                ("ForEach","ForEach"),
                ("Parrell","Parrell")
            };

            if(data.ContainsKey("Inputs") == false )
            {
                data.Add("Inputs", new List<Dictionary<String, Object>>());
            }
            if (data.ContainsKey("Outputs") == false)
            {
                data.Add("Outputs", new List<Dictionary<String, Object>>());
            }
        }

        private async Task AddInputParam()
        {

        }
        private async Task EditParam(Dictionary<String,Object> param)
        {

        }

        private async Task StepTypeChanged(String v)
        {
            data.getList("Inputs").Clear();
            switch (v)
            {
                case "Event":
                    Dictionary<String,Object> param = new Dictionary<String,Object>();
                    param.Add("Source", "EventKey");
                    param.Add("Target", "");
                    param.Add("Type", "System");

                    Dictionary<String, Object> param1 = new Dictionary<String, Object>();
                    param1.Add("Source", "EventName");
                    param1.Add("Target", "");
                    param1.Add("Type", "System");

                    data.getList("Inputs").Add(param);
                    data.getList("Inputs").Add(param1);
                    break;
                case "While":
                    break;
                case "ForEach":
                    break;
                case "Parrell":
                    break;
                default: break;
            }
        }
    }
}
