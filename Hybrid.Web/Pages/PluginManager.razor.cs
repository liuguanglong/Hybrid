using Hybrid.Web.PlugIn;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Hybrid.Web.Service;

namespace Hybrid.Web.Pages
{
    public partial class PluginManager
    {
        private List<Type?> components = new();
        private IEnumerable<Package> packages;

        [Inject]
        public PluginStateService pluginService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            packages = pluginService.GetPackages();
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            try
            {
                await repo.Upload(e.File.Name, e.File.OpenReadStream());
                Package p = new Package();
                p.Name = e.File.Name;
                await Load(p);
                pluginService.AddPackage(p);
                packages = pluginService.GetPackages();

                Snackbar.Add("Plugin is succssfully added!", Severity.Info);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Exception ocurred when trying to add Plugin!", Severity.Info);
            }
        }

        private async Task Load(Package package)
        {
            await repo.Load(package);
        }

        private async Task Remove(Package package)
        {
            try
            {
                await repo.Delete(package);
                pluginService.RemovePackage(package);
                packages = pluginService.GetPackages();

                Snackbar.Add("Plugin is succssfully removed!", Severity.Info);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Exception ocurred when trying to remove Plugin!", Severity.Info);
            }
        }

        private void LoadComponent(ChangeEventArgs changeEventArgs, Package package)
        {
            string component = changeEventArgs.Value?.ToString() ?? "";

            components.Add(package.Assembly?.GetType(component));

            foreach (var asset in package.Assets)
            {
                var id = package.Name + asset.Item2.Substring(0, asset.Item2.LastIndexOf("."));

                if (asset.Item1 == "css")
                {
                    if (File.Exists($"/_content/{package.Name}/{asset.Item2}"))
                        DOMinterop.IncludeLink(id, $"/_content/{package.Name}/{asset.Item2}");
                }
                else if (asset.Item1 == "js")
                {
                    if (File.Exists($"/_content/{package.Name}/{asset.Item2}"))
                        DOMinterop.IncludeScript(id, $"/_content/{package.Name}/{asset.Item2}");
                }
            }
        }
    }
}
