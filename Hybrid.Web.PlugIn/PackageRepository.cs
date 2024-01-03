using Supabase.Storage;
using Supabase.Storage.Interfaces;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.Loader;
using System.Xml;

namespace Hybrid.Web.PlugIn
{
    public class PackageRepository : IPackageRepository
    {
        private List<Package> packages = new();
        private String BucketName = "BlazorPlugIn";
        private readonly IStorageClient<Supabase.Storage.Bucket, Supabase.Storage.FileObject> _storage;

        public PackageRepository(Supabase.Client client)
        {
            this._storage = client.Storage;
        }

        public async Task<List<Package>> GetList()
        {
            if (packages.Count == 0)
            {
                var result = await _storage.From(BucketName).List();

                List<Package> packages = new();
                foreach (var fileObj in result)
                {
                    if(fileObj.IsFolder == true)
                        packages.Add(new Package { Name = Path.GetFileName(fileObj.Name) });
                }
                return packages;
            }

            return packages;
        }

        public bool CheckLoaded(string package)
        {
            return packages.Any(s => s.Name == package && s.IsLoaded);
        }

        public async Task Upload(String packageName,Stream fileStream)
        {
            var bucket = _storage.From(BucketName);
            var validFormats = new string[] { ".dll", ".pdb", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".json", ".txt", ".csv" };

            using MemoryStream ms = new();
            await fileStream.CopyToAsync(ms);

            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Read,false))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (validFormats.Contains(Path.GetExtension(entry.Name))
                        || entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props") // Static content specification
                    {
                        string path = Path.Combine(packageName, entry.Name);
                        using Stream zipStream = entry.Open();
                        var bytesData = await StreamToBytesAsync(zipStream);
                        await bucket.Upload(bytesData, path);
                    }
                }
            }
        }

        public async Task<byte[]> StreamToBytesAsync(Stream streamData)
        {
            byte[] bytes;

            using var memoryStream = new MemoryStream();
            await streamData.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();

            return bytes;
        }

        public async Task<bool> Delete(Package package)
        {
            //if (package.Name != null && CheckLoaded(package.Name)) return true;

            var bucket = _storage.From(BucketName);
            try
            {
                SearchOptions options = new SearchOptions();
                var list = await bucket.List(package.Name);
                foreach(var item in list)
                {
                    var ret = await bucket.Remove($"{package.Name}/{item.Name}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception occurred when trying to delete plugin! {package.Name}");
            }
            return true;
        }

        public async Task<bool> Load(Package package)
        {
            if (package.Name != null && CheckLoaded(package.Name)) return true;
            var bucket = _storage.From(BucketName);

            try
            {
                var packageName = package.Name.Split(new char[] { '.' })[0];

                var bytes = await bucket.Download($"{package.Name}/{packageName}.dll", 
                    (_, f) => Debug.WriteLine($"Download Progress: {f}%"));
                using MemoryStream stream = new MemoryStream(bytes);
                var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
                package.Assembly = assembly;
                try
                {
                    var bytes2 = await bucket.Download($"{package.Name}/{packageName}.pdb",
                        (_, f) => Debug.WriteLine($"Download Progress: {f}%"));

                    var stream2 = new MemoryStream(bytes2);
                    var symbols = AssemblyLoadContext.Default.LoadFromStream(stream2);
                    package.Symbols = symbols;
                }
                catch
                {
                    Console.WriteLine($"No symbols loaded for {package.Name}");
                }
                package.Components = assembly.GetExportedTypes().Where(x=>x.BaseType?.Name == "ComponentBase")
                    .Select(s => (s.FullName ?? "", s.BaseType?.Name ?? "")).ToList();
                package.IsLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            var bytes3 = await bucket.Download($"{package.Name}/Microsoft.AspNetCore.StaticWebAssets.props",
                (_, f) => Debug.WriteLine($"Download Progress: {f}%"));

            // Find List of assets to load
            var stream3 = new MemoryStream(bytes3);
            XmlDocument assetsList = new XmlDocument();
            assetsList.Load(stream3);
            foreach (XmlNode asset in assetsList.GetElementsByTagName("StaticWebAsset"))
            {
                var content = asset.SelectSingleNode("RelativePath")?.InnerText;
                if (content.EndsWith(".js"))
                    package.Assets.Add(("js", content));
                else if (content.EndsWith(".css"))
                    package.Assets.Add(("css", content));
            }

            return true;
        }
    }
}
