namespace FileWatcherService
{
    public interface IFileObserver
    {
        void Start();
        void Stop();
    }
}