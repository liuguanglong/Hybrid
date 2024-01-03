using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.FormEngine
{
    public class UIControlStub : UIElementStub
    {
        public RenderFragment Control { get; set; }

        public UIControlStub(string name, string label, RenderFragment control) : base(name, label)
        {
            Control = control;
        }
        public override RenderFragment Render()
        {
            return Control;
        }
    }

}
