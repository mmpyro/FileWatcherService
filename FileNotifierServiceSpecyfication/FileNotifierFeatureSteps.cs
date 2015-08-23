using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading;
using FileWatcherService;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace FileNotifierServiceSpecyfication
{
    [Binding]
    public class FileNotifierFeatureSteps
    {
        const string FilePath = @"D:\test\a.txt";
        readonly string NotifierlogTxt = Path.Combine(Path.GetTempPath(), "Notifierlog.txt");
        const string PathToObserve = @"D:\test\";
        private Process _service;
        private IFileNotifierManager _proxy;
        private ChannelFactory<IFileNotifierManager> _channel;

        [BeforeScenario]
        public void Init()
        {
            DeleteIfExists(NotifierlogTxt);
        }

        [Given(@"I have the fileNotifier service")]
        public void GivenIHaveTheFileNotifierService()
        {
            _service = Process.Start(@"D:\Projects\Codes\FileNotifier\ConsoleWatcher\bin\Debug\ConsoleWatcher.exe");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            CreateProxy();
        }
        
        [Given(@"I set path to observe")]
        public void GivenISetPathToObserve()
        {
            _proxy.Set(new ObserveFileDto(PathToObserve));
        }

        [When(@"I create file")]
        public void WhenICreateFile()
        {
            File.Create(FilePath).Close();
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [Then(@"notification was recived")]
        public void ThenNotificationWasRecived()
        {
            StopService();
            Assert.True(File.Exists(NotifierlogTxt));
        }

        [When(@"I remove path to observe")]
        public void WhenIRemovePathToObserve()
        {
            _proxy.Remove(PathToObserve);
        }

        [Then(@"notification wasn't recived")]
        public void ThenNotificationWasnTRecived()
        {
            StopService();
            Assert.False(File.Exists(NotifierlogTxt));
        }


        [AfterScenario]
        public void Clean()
        {
            DeleteIfExists(FilePath);
            DeleteIfExists(NotifierlogTxt);
        }

        private void StopService()
        {
            _channel.Close();
            _service.Kill();
        }

        private void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void CreateProxy()
        {
            _channel = new ChannelFactory<IFileNotifierManager>(new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/FileWatcherService"));
            _proxy = _channel.CreateChannel();
        }
    }
}
