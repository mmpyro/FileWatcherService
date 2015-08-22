using System.IO;
using FileNotifier;
using NSubstitute;
using NUnit.Framework;

namespace FileNotifierSpecyfication
{
    internal class FakeNotifier : IFileNotifier
    {
        private bool _wasInvoked = false;

        public bool WasInvoked
        {
            get { return _wasInvoked; }
        }

        public void OnCreated(FileSystemEventArgs arg)
        {
            _wasInvoked = true;
        }

        public void OnRename(RenamedEventArgs arg)
        {
            _wasInvoked = true;
        }
    }

    [TestFixture]
    public class ObserverFileBoxSpecification
    {
        const string FilePath = @"D:\myTestFile.dat";

        [TestFixtureSetUp]
        public void Init()
        {
            FileObserver.CreateFunction = (dto, notifier) => new FileWatchDog(dto, notifier);
        }

        [Test]
        [Category("Box")]
        public void NotifyFileOnCreated_Test()
        {
            //Given
            var fileNotifier = new FakeNotifier();
            var fileToObserve = new ObserveFileDto()
            {
                DirectoryPath = @"D:",
                Filter = "*.dat",
                WithSubDirectories = false
            };
            IFileObserver adapter = FileObserver.Create(fileToObserve, fileNotifier);
            //When
            adapter.Start();
            File.Create(FilePath).Close();
            File.Delete(FilePath);
            //Then
            Assert.That(fileNotifier.WasInvoked, Is.True);
        }

        [TearDown]
        public void After()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}