using Hybrid.Web.PlugIn;

namespace Hybrid.Web.Service
{
    public class PluginStateService
    {
        public static Dictionary<String,Package> packages = new Dictionary<String,Package>();

        public void AddPackage(Package package)
        {
            packages.Add(package.Name, package);
        }

        public void RemovePackage(Package package)
        {
            packages.Remove(package.Name);
        }

        public IEnumerable<Package> GetPackages()
        {
            return packages.Select(x=>x.Value);
        }
    }
}
