
using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class UIElement
    {
        public string Name { get; set; }
        public string DataPath { get; set; }
        public string Label { get; set; }
        public int Index { get; set; } = 0;
        public String Id { get; set; }

        public UIElement(Dictionary<string, Object> data)
        {
            this.Name = data.getValue<String>("name");
            this.Label = data.getValue<String>("label");
            this.Id = data.getValue<String>("id");
            this.DataPath = data.getValue<String>("data_path");
            this.Index = data.getValue<Int32>("index");
        }

        public UIElement(string name, string label, string dataPath)
        {
            Name = name;
            Label = label;
            DataPath = dataPath;
        }
    }
}
