using System;
using System.Collections.Generic;

namespace FileNotifierSpecyfication
{
    public class FileNotifierManager
    {
        private readonly IFileNotifier _fileNotifier;

        public FileNotifierManager(IFileNotifier fileNotifier)
        {
            _fileNotifier = fileNotifier;
            ObservedFiles = new Dictionary<string, IFileObserver>();
        }

        public Dictionary<string,IFileObserver> ObservedFiles { get; protected set; }

        public void Set(ObserveFileDto fileToObserve)
        {
            IFileObserver adapter = FileObserver.Create(fileToObserve, _fileNotifier);
            AddToObserverList(fileToObserve,adapter);
        }

        private void AddToObserverList(ObserveFileDto fileToObserve, IFileObserver adapter)
        {
            if (!ObservedFiles.ContainsKey(fileToObserve.Path))
            {
                adapter.Start();
                ObservedFiles.Add(fileToObserve.Path, adapter);
            }
            else
                throw new ArgumentException("This path was added to observable list before.");
        }
    }

    public class FileObserver
    {
        public static Func<ObserveFileDto, IFileNotifier, IFileObserver> CreateFunction { get; set; }

        public static IFileObserver Create(ObserveFileDto fileToObserve, IFileNotifier fileNotifier)
        {
            return CreateFunction(fileToObserve, fileNotifier);
        }
    }

    public interface IFileObserver
    {
        void Start();
    }
}