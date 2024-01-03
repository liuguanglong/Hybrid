
using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcScreen
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BcBreadcrumb[] BreadcrumbItems { get; set; } = new BcBreadcrumb[0];
        public Dictionary<String, BcPart> UIParts { get; set; } = new Dictionary<String, BcPart>();

        public BcScreen() { }

        public BcScreen(Dictionary<String, Object> data) {
            Dictionary<String, Object> screen = data.getDictionary("screen[0]");
            if (screen != null)
            {
                this.Id = screen.getValue<String>("id");
                this.Name = screen.getValue<String>("name");

                var listpart = screen.getList("parts");
                foreach(var item in listpart )
                {
                    var p = new BcPart(item);
                    UIParts.Add(p.Name, p);
                }

                foreach(var p in UIParts.Values)
                {
                    foreach(var e in p.Elements)
                    {
                        if(e is BcList)
                        {
                            BcList l = (BcList)e;
                            l.part = UIParts.Values.Where(x=>x.Id== l.partId).FirstOrDefault();
                        }
                    }
                }

                var listBreadcrumbs = screen.getList("breadcrumbs");
                BreadcrumbItems = new BcBreadcrumb[listBreadcrumbs.Count];
                foreach (var item in listBreadcrumbs)
                {
                    var p = new BcBreadcrumb(item);
                    if (p.Index >= 0 && p.Index < BreadcrumbItems.Length)
                        BreadcrumbItems[p.Index] = p;
                }
            }
        }
    }
}
