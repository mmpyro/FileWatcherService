using System;
using NSubstitute;
using NUnit.Framework;

namespace FileNotifierSpecyfication
{
    public class FileNotifierServiceSpecification
    {
        [TestFixtureSetUp]
        public void Before()
        {
            FileObserver.CreateFunction = (dto, notifier) => Substitute.For<IFileObserver>();
        }

        [Test]
        public void AddFileToObservableList_Test()
        {
            //Given
            var mockNotifier = Substitute.For<IFileNotifier>();
            var fileNotifierManager = new FileNotifierManager(mockNotifier);
            var fileToObserve = new ObserveFileDto()
            {
                Path = @"D:\data.xml",
                Filter = string.Empty,
                WithSubDirectories = true
            };
            //When
            fileNotifierManager.Set(fileToObserve);
            //Then
            Assert.AreEqual(fileNotifierManager.ObservedFiles.Count, 1);
        }
 
        [Test]
        public void AddFileToObservableListMoreThanOnce_Test()
        {
            //Given
            var mockNotifier = Substitute.For<IFileNotifier>();
            var fileNotifierManager = new FileNotifierManager(mockNotifier);
            var fileToObserve = new ObserveFileDto()
            {
                Path = @"D:\data.xml",
                Filter = string.Empty,
                WithSubDirectories = true
            };
            //When
            fileNotifierManager.Set(fileToObserve);
            //Then
            Assert.Throws<ArgumentException>(() => fileNotifierManager.Set(fileToObserve));
        }
    }
}