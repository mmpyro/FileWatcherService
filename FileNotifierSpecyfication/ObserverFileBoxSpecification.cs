﻿using System.IO;
using FileNotifier;
using FileWatcherService;
using NUnit.Framework;

namespace FileNotifierSpecyfication
{
    internal class FakeNotifier : IFileNotifier
    {
        private int _countInvoked = 0;

        public int CountInvoked
        {
            get { return _countInvoked; }
        }

        public void OnCreated(FileSystemEventArgs arg)
        {
            _countInvoked += 1;
        }

        public void OnRename(RenamedEventArgs arg)
        {
            _countInvoked += 1;
        }
    }

    [TestFixture]
    public class ObserverFileBoxSpecification
    {
        const string FilePath = @"D:\test\myTestFile.dat";
        const int Delay = 1000;
        const string SubDirPath = @"D:\test\subdir";

        [TestFixtureSetUp]
        public void Init()
        {
            FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto,notifier );
            if (!Directory.Exists(SubDirPath))
            {
                Directory.CreateDirectory(SubDirPath);
            }
        }

        [Test]
        [Category("Box")]
        public void NotifyFileOnCreated_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = false
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            File.Create(FilePath).Close();
            //Then
            Assert.That( () => fileNotifier.CountInvoked, Is.EqualTo(1).After(Delay));
        }

        [Test]
        [Category("Box")]
        public void NotifyFileOnDeleted_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = false
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            File.Create(FilePath).Close();
            File.Delete(FilePath);
            //Then
            Assert.That(() => fileNotifier.CountInvoked, Is.EqualTo(2).After(Delay));
        }

        [Test]
        [Category("Box")]
        public void NotifyOnChanged_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = false
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            File.Create(FilePath).Close();
            CreateAndFillFile(FilePath);
            //Then
            Assert.That(() => fileNotifier.CountInvoked, Is.EqualTo(2).After(Delay));
        }

        [Test]
        [Category("Box")]
        public void OnRename_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = false
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            File.Create(FilePath).Close();
            const string newfileName = @"D:\test\newFileName.txt";
            File.Move(FilePath, newfileName);
            DeleteIfExist(newfileName);
            //Then
            Assert.That(() => fileNotifier.CountInvoked, Is.EqualTo(3).After(Delay));
        }

        [Test]
        [Category("Box")]
        public void NotifyInSubDirectory_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = true
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            const string testfile = "testfile.txt";
            File.Create(Path.Combine(SubDirPath, testfile)).Close();
            //Then
            Assert.That(() => fileNotifier.CountInvoked, Is.EqualTo(1).After(Delay));
        }

        [Test]
        [Category("Box")]
        public void RemoveFromObservableList_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:\test",
                Filter = "*.*",
                WithSubDirectories = false
            };
            var fileNotifierManager = new FileNotifierManager(fileNotifier);
            //When
            fileNotifierManager.Set(fileToObserve);
            File.Create(FilePath).Close();
            fileNotifierManager.Remove(fileToObserve.DirectoryPath);
            DeleteIfExist(FilePath);
            //Then
            Assert.That(() => fileNotifier.CountInvoked, Is.EqualTo(1).After(Delay));
        }

        [TearDown]
        public void After()
        {
            DeleteIfExist(FilePath);
        }

        [TestFixtureTearDown]
        public void Clear()
        {
            if (Directory.Exists(SubDirPath))
            {
                Directory.Delete(SubDirPath, true);
            }
        }

        private static void DeleteIfExist(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        private void CreateAndFillFile(string path)
        {
            using (var sw = new StreamWriter(path, true))
            {
                sw.WriteLine("Some short string");
            }
        }

    }
}