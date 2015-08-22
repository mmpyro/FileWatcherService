using System.Collections.Generic;

namespace FileNotifier
{
    public interface IFileNotifierManager
    {
        void Set(ObserveFileDto fileToObserve);
        void Remove(string filePath);
        List<ObserveFileDto> PerformFileList();
    }
}