using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileWatcherService;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;

namespace ConsoleFileWatcherService
{
    class Program
    {
        private static Logger _logger;
        public static FileNotifierManager _fileManager;
        private const string URL = "http://localhost:8080";

        static void Main(string[] args)
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
                FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
                _fileManager = new FileNotifierManager(new SignalRNotifier(),new ConsoleNotifier(_logger));
                _fileManager.Set(new ObserveFileDto(@"D:\test\"));
                using (WebApp.Start(URL))
                {
                    _logger.Info("Service started");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            finally
            {
                _logger.Info("Service stoped");
                Console.WriteLine("Press any key ...");
                Console.ReadKey();
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(FileNotifierHub), () => new FileNotifierHub(Program._fileManager) );
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
