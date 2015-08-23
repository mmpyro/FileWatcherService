using System;
using System.IO;
using FileNotifier;

namespace ConsoleWatcher
{
    internal class FakeNotifier : IFileNotifier
    {

        public void OnCreated(FileSystemEventArgs arg)
        {
            Console.WriteLine(arg);
        }

        public void OnRename(RenamedEventArgs arg)
        {
            Console.WriteLine(arg);
        }
    }
}