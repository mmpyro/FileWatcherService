using System.Collections.Generic;

namespace FileWatcherService
{
    public interface IFileNotifierManager
    {
        void Set(ObserveFileDto fileToObserve);

        void Remove(string filePath);

        List<ObserveFileDto> PerformFileList();
    }
}