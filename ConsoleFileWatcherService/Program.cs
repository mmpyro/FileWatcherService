using System;
using NLog;
using Topshelf;

namespace FileWatcherService
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = null;
            try
            {
                logger = LogManager.GetCurrentClassLogger();
                HostFactory.Run(serviceConfig =>
                {
                    serviceConfig.Service<FileWatcherService>(serviceInstance =>
                    {
                        serviceInstance.ConstructUsing(() => new FileWatcherService(logger));
                        serviceInstance.WhenStarted(service => service.StartService());
                        serviceInstance.WhenStopped(service => service.StopService());
                    });
                    serviceConfig.EnableServiceRecovery(recovery =>
                    {
                        recovery.RestartService(1);
                        recovery.RestartService(1);
                        recovery.RestartService(1);
                    });
                    serviceConfig.SetServiceName("FileWatcherService");
                    serviceConfig.SetDisplayName("FileWatcherService");
                    serviceConfig.SetDescription("Service for watching files");
                    serviceConfig.StartAutomatically();
                });
            }
            catch (Exception e)
            {
                if(logger != null)
                    logger.Error(e);
            }
        }
    }
}
