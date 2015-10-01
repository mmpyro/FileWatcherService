using FileWatcherService;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

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

        public void RemoveObservedPath(string path)
        {
            _fileNotifierManager.Remove(path);
        }

        public void AddFileToObserverPath(string json)
        {
            var fileDto = JsonConvert.DeserializeObject<ObserveFileDto>(json);
            _fileNotifierManager.Set(fileDto);
        }

        public void Send(string message)
        {
            Clients.All.notify(message);
        }
    }
}