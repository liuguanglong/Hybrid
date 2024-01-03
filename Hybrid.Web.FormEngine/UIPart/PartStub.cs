using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.FormEngine
{
    public class PartStub
    {
        public string Name { get; set; }
        public string Label { get; set; }

        public String Width { get; set; } = "80%";
        public String Height { get; set; } = "60%";

        public int Index { get; set; }
        public Dictionary<string, RenderFragment> Buttons { get; set; }
        public Dictionary<string, UIElementStub> Elements { get; set; } = new Dictionary<string, UIElementStub>();
    }

}
