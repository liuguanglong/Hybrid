using MudBlazor;
using AnyClone;

namespace Hybrid.Web.Pages.Workflow
{
    public partial class CreateWorkflow
    {
        MudForm form;

        Dictionary<String,Object> data = new Dictionary<String,Object>();

        protected override async Task OnParametersSetAsync()
        {
            data.Clear();
            data.Add("Id",Guid.NewGuid().ToString());
            data.Add("Name", "HelloWorld");
            data.Add("Version", "1");

            Dictionary<String,Object> step1 = new Dictionary<String,Object>();
            step1.Add("Id", Guid.NewGuid().ToString());
            step1.Add("Index", 0);
            step1.Add("Name", "Helloworld");
            step1.Add("StepType", "Event");
            step1.Add("NextStepId", "");

            data.Add("Steps", new List<Dictionary<String, Object>>() { step1});
        }

        async Task EditStep(Dictionary<String,Object> data)
        {
            Dictionary<string, Object> value = data.Clone();

            var parameters = new DialogParameters<EditStep>();
            parameters.Add(x => x.data, value);
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            var ret = DialogService.Show<EditStep>("Edit Step",parameters, options);
        }

        async Task AddStep()
        {

        }
    }
}
