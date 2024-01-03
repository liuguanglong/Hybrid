using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcPart
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Label { get; set; }
        public int Index { get; set; }
        public bool mainLevel { get; set; }

        public String Width { get; set; } = "80%";
        public String Height { get; set; } = "60%";

        public UIElement[] Elements { get; set; }
        public BcButton[] Buttons { get; set; } = new BcButton[0];

        public BcPart(String name, String label, String datapath)
        {

        }

        public BcPart(Dictionary<String, Object> data)
        {
            this.Id = data.getValue<String>("id");
            this.Name = data.getValue<String>("name");
            this.Label = data.getValue<String>("label");
            this.Index = data.getValue<Int32>("index");
            this.mainLevel = data.getValue<bool>("is_main_part");
            this.Width = data.getValue<String>("width");
            this.Height = data.getValue<String>("height");

            var listInputs = data.getList("inputs");
            var listLists = data.getList("list");
            var listLinkes = data.getList("links");
            var listfields = data.getList("fields");
            var listbuttons = data.getList("buttons");
            var listCheckboxs = data.getList("checkboxs");
            var listSelects = data.getList("selects");
            var listTrees = data.getList("trees");

            Elements = new UIElement[listInputs.Count + listLists.Count + listLinkes.Count + listfields.Count 
                + listCheckboxs.Count + listSelects.Count + listTrees.Count];

            if (listTrees != null)
            {
                foreach (var item in listTrees)
                {
                    var input = new BcTree(item);
                    if (input.Index >= 0 && input.Index < Elements.Length)
                        Elements[input.Index] = input;
                }
            }

            if (listSelects != null)
            {
                foreach (var item in listSelects)
                {
                    var input = new BcSelect(item);
                    if (input.Index >= 0 && input.Index < Elements.Length)
                        Elements[input.Index] = input;
                }
            }

            if (listCheckboxs != null)
            {
                foreach (var item in listCheckboxs)
                {
                    var input = new BcCheckBox(item);
                    if (input.Index >= 0 && input.Index < Elements.Length)
                        Elements[input.Index] = input;
                }
            }

            if (listfields != null)
            {
                foreach (var item in listfields)
                {
                    var input = new BcField(item);
                    if (input.Index >= 0 && input.Index < Elements.Length)
                        Elements[input.Index] = input;
                }
            }

            if (listInputs != null)
            {
                foreach (var item in listInputs)
                {
                    var input = new BcInput(item);
                    if (input.Index >= 0 && input.Index < Elements.Length)
                        Elements[input.Index] = input;
                }
            }

            if (listLists != null)
            {
                foreach (var item in listLists)
                {
                    var list = new BcList(item);
                    if (list.Index >= 0 && list.Index < Elements.Length)
                        Elements[list.Index] = list;
                }

            }

            if(listLinkes !=null)
            {
                foreach (var item in listLinkes)
                {
                    var link = new BcLink(item);
                    if (link.Index >= 0 && link.Index < Elements.Length)
                        Elements[link.Index] = link;
                }
            }

            if (listbuttons.Count > 0)
            {
                Buttons = new BcButton[listbuttons.Count];
                foreach (var button in listbuttons)
                {
                    var b = new BcButton(button);
                    if (b.Index >= 0 && b.Index < listbuttons.Count)
                        Buttons[b.Index] = b;
                }
            }

        }
    }
}
