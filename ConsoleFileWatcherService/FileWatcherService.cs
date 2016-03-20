using FileNotifier;
using FileWatcher;
using FileWatcherService.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace FileWatcherService
{
    class FileWatcherService : IFileWatcherService
    {
        private Logger _logger;
        private const string URL = "http://localhost:8080";
        private IDisposable _webApp;

        public FileWatcherService(Logger logger)
        {
            _logger = logger;
            FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
            var fileManager = new FileNotifierManager(CreateNotifierList());
            GlobalHost.DependencyResolver.Register(typeof(FileNotifierHub), () => new FileNotifierHub(fileManager));
        }

        public void StartService()
        {
            string url = ConfigurationManager.AppSettings["Url"] ?? URL;
            _webApp = WebApp.Start(url);
            _logger.Info("Service started");
        }

        public void StopService()
        {
            if (_webApp != null)
                _webApp.Dispose();
        }

        private IFileNotifier[] CreateNotifierList()
        {
            PluginManager pluginManager = new PluginManager(Path.Combine(Directory.GetCurrentDirectory(), "Plugins"));
            List<IFileNotifier> notifiersList = new List<IFileNotifier>();
            notifiersList.Add(new SignalRNotifier());
            notifiersList.AddRange(pluginManager.GetPlugins());
            return notifiersList.ToArray();
        }
        
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
