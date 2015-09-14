using FileWatcherService;
using Microsoft.AspNet.SignalR;

namespace ConsoleFileWatcherService
{
    public class FileNotifierHub : Hub
    {
        private readonly FileNotifierManager _fileNotifierManager;

        public FileNotifierHub(FileNotifierManager fileNotifierManager)
        {
            _fileNotifierManager = fileNotifierManager;
        }

        public void GetObservedPaths()
        {
            Clients.Caller.GetObservedPaths(_fileNotifierManager.PerformFileList());
        }

        public void Send(string message)
        {
            Clients.All.notify(message);
        }
    }
}