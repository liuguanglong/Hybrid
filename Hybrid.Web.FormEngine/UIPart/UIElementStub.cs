using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.FormEngine
{
    public class UIElementStub
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public UIElementStub(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public virtual RenderFragment Render()
        {
            throw new NotImplementedException();
        }
    }
}
