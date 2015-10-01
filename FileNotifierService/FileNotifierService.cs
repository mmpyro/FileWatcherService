using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FileWatcher;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;

namespace FileNotifierService
{
    public partial class FileNotifierService : ServiceBase
    {
        private static Logger _logger;
        public static FileNotifierManager _fileManager;
        private const string URL = "http://localhost:8080";

        public FileNotifierService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
                FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
                _fileManager = new FileNotifierManager(new SignalRNotifier());
                _fileManager.Set(new ObserveFileDto(@"D:\test\"));
                using (WebApp.Start(URL))
                {
                    _logger.Info("Service started");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        protected override void OnStop()
        {
            if (_logger != null)
            {
                _logger.Info("Stop service");
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(FileNotifierHub), () => new FileNotifierHub(FileNotifierService._fileManager));
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
