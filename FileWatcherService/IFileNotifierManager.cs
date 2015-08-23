using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace FileWatcherService
{
    [ServiceContract]
    public interface IFileNotifierManager
    {
        [OperationContract(IsOneWay = true)]
        void Set(ObserveFileDto fileToObserve);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void Remove(string filePath);

        [OperationContract]
        List<ObserveFileDto> PerformFileList();
    }
}