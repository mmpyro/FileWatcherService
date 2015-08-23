using System;
using FileNotifier;

namespace FileWatcherService
{
    public class FileObserver
    {
        public static Func<ObserveFileDto, IFileNotifier[], IFileObserver> CreateFunction { get; set; }

        public static IFileObserver Create(ObserveFileDto fileToObserve, IFileNotifier[] fileNotifier)
        {
            return CreateFunction(fileToObserve, fileNotifier);
        }
    }
}