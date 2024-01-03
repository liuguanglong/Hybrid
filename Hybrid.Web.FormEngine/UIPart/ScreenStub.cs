using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Web.FormEngine
{
    public class ScreenStub
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string AuthorizeRole { get; set; } = "UIDesigner,Manager";
        public RenderFragment Breadcrumb { get; set; }
        public Dictionary<string, PartStub> Parts { get; set; } = new Dictionary<string, PartStub>();
    }
}
