using FileNotifier;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FileWatcherService.Core
{
    public class PluginManager
    {
        private CompositionContainer _container;
        private readonly string _path;

        public PluginManager(string path)
        {
            _path = path;
            LoadPlugins();
        }
        
        public IEnumerable<IFileNotifier> GetPlugins()
        {
            return _container.GetExportedValues<IFileNotifier>();
        }

        private void LoadPlugins()
        {
            var catalog = new AggregateCatalog();
            foreach( var file in  Directory.GetFiles(_path, "*.dll"))
            {
                try
                {
                    var ac = new AssemblyCatalog(Assembly.LoadFile(file));
                    ac.Parts.ToArray();
                    catalog.Catalogs.Add(ac);
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }
            _container = new CompositionContainer(catalog);
        }
    }
}
