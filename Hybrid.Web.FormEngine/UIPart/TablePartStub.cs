using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Web.FormEngine
{
    public class TablePartStub : UIElementStub
    {
        public TablePartStub(string name, string label) : base(name, label)
        {
        }

        public Dictionary<string, RenderFragment> Buttons { get; set; }
        public List<RenderFragment> Headers { get; set; }
        public List<Dictionary<string, RenderFragment>> Rows { get; set; } = new List<Dictionary<string, RenderFragment>>();

    }
}
