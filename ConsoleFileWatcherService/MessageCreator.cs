using System;
using System.IO;

namespace ConsoleFileWatcherService
{
    public static class MessageCreator
    {
        public static string PerformMessage(FileSystemEventArgs arg)
        {
            return string.Format("[{0}]: {1}, {2}", DateTime.Now, arg.ChangeType, arg.FullPath);
        }

        public static string PerformMessage(RenamedEventArgs arg)
        {
            return string.Format("[{0}]: {1} from {3} to {4}, {2}", DateTime.Now, arg.ChangeType, arg.FullPath,
                arg.OldName, arg.Name);
        }

    }
}