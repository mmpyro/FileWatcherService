using System;
using System.IO;
using FileNotifier;
using Microsoft.AspNet.SignalR;

namespace ConsoleFileWatcherService
{
    internal class SignalRNotifier : IFileNotifier
    {
        private readonly IHubContext _hubContext;

        public SignalRNotifier()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<FileNotifierHub>();
        }

        public void OnCreated(FileSystemEventArgs arg)
        {

            _hubContext.Clients.All.notify(MessageCreator.PerformMessage(arg));
        }

        public void OnRename(RenamedEventArgs arg)
        {
            _hubContext.Clients.All.notify(MessageCreator.PerformMessage(arg));
        }
    }
}