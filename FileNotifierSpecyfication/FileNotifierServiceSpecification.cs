using System;
using FileNotifier;
using FileWatcher;
using NSubstitute;
using NUnit.Framework;

namespace FileNotifierSpecyfication
{
    [TestFixture]
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
                DirectoryPath = @"D:\data.xml",
                Filter = string.Empty,
                WithSubDirectories = true
            };
            //When
            fileNotifierManager.Set(fileToObserve);
            //Then
            Assert.AreEqual(fileNotifierManager.PerformFileList().Count, 1);
        }
 
        [Test]
        public void AddFileToObservableListMoreThanOnce_Test()
        {
            //Given
            var mockNotifier = Substitute.For<IFileNotifier>();
            var fileNotifierManager = new FileNotifierManager(mockNotifier);
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\data.xml",
                Filter = string.Empty,
                WithSubDirectories = true
            };
            //When
            fileNotifierManager.Set(fileToObserve);
            //Then
            Assert.Throws<ArgumentException>(() => fileNotifierManager.Set(fileToObserve));
        }

        [Test]
        public void RemoveFromObservableList_Test()
        {
            //Given
            var mockNotifier = Substitute.For<IFileNotifier>();
            var fileNotifierManager = new FileNotifierManager(mockNotifier);
            const string path = @"D:\data.xml";
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = path,
                Filter = string.Empty,
                WithSubDirectories = true
            };
            //When
            fileNotifierManager.Set(fileToObserve);
            fileNotifierManager.Remove(path);
            //Then
            Assert.AreEqual(fileNotifierManager.PerformFileList().Count, 0);
        }

        [Test]
        public void RemoveFromObservableListException_Test()
        {
            //Given
            var mockNotifier = Substitute.For<IFileNotifier>();
            var fileNotifierManager = new FileNotifierManager(mockNotifier);
            const string path = @"D:\data.xml";

            //Then
            Assert.Throws<InvalidOperationException>(() => fileNotifierManager.Remove(path));
        }
    }
}