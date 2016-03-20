using FileWatcher;
using FileWatcherService.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;
using System;
using System.Configuration;

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
            var fileManager = new FileNotifierManager(new SignalRNotifier());
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
