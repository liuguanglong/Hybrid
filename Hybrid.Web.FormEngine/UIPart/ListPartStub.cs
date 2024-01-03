using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.FormEngine
{
    public class ListPartStub : UIElementStub
    {
        public Dictionary<string, RenderFragment> Buttons { get; set; }
        public List<Dictionary<string, UIControlStub>> Elements { get; set; } = new List<Dictionary<string, UIControlStub>>();
        public int PageCount { get; set; } = 3;
        public int PageIndex { get; set; } = 1;

        public String Width { get; set; } = "80%";
        public String Height { get; set; } = "60%";
        public String PartWidth { get; set; } = "200px";
        public String PartHeight { get; set; } = "400px";

        public ListPartStub(string name, string label, List<Dictionary<string, UIControlStub>> elements) : base(name, label)
        {
            Elements = elements;
        }

        public int getCount()
        {
            int pageCount = Elements.Count / PageCount;
            if(Elements.Count % PageCount > 0)
            {
                pageCount++;
            }
            return pageCount;
        }
    }

}
