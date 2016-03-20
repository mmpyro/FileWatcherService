using System.IO;
using FileNotifier;
using NLog;

namespace FileWatcherService.Core
{
    internal class ConsoleNotifier : IFileNotifier
    {
        private readonly Logger _logger;

        public ConsoleNotifier(Logger logger)
        {
            _logger = logger;
        }

        public void OnCreated(FileSystemEventArgs arg)
        {
            _logger.Info(MessageCreator.PerformMessage(arg));
        }

        public void OnRename(RenamedEventArgs arg)
        {
            _logger.Info(MessageCreator.PerformMessage(arg));
        }
    }
}