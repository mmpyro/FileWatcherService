using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.ServiceModel;
using FileNotifier;
using FileWatcherService;

namespace ConsoleWatcher
{
    public class PluginLoader
    {
        [ImportMany(typeof (IFileNotifier))]
        public IEnumerable<IFileNotifier> Notifiers { get; set; }


        public void LoadPlugins()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(Directory.GetCurrentDirectory(), "Plugins")));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var pluginLoader = new PluginLoader();
                pluginLoader.LoadPlugins();
                var notifiers = new List<IFileNotifier>(pluginLoader.Notifiers);
                notifiers.Add(new FakeNotifier());
                FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifiers.ToArray());
                IFileNotifierManager fileNotifierManager = new FileNotifierManager(new FakeNotifier());
                using (var service = new ServiceHost(fileNotifierManager))
                {
                    service.Open();
                    fileNotifierManager.Set(new ObserveFileDto(@"D:\test"));
                    Console.WriteLine("Service Started");
                    Console.WriteLine("To stop service press any key ...");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Service Stoped");
                Console.ReadKey();
            }
        }

    }
}
