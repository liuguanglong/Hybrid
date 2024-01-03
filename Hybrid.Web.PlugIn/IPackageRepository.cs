namespace Hybrid.Web.PlugIn
{
    public interface IPackageRepository
    {
        bool CheckLoaded(string package);
        Task<List<Package>> GetList();
        Task<bool> Load(Package package);
        Task Upload(String packageName,Stream fileStream);
        Task<bool> Delete(Package package);
    }
}
