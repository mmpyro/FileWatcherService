using System;
using System.ComponentModel.Composition;
using System.IO;
using FileNotifier;

namespace TextFileNotifierPlugin
{
    [Export(typeof(IFileNotifier))]
    public class TextFileNotifierPlugin : IFileNotifier
    {
        private readonly string _fileName;

        public TextFileNotifierPlugin()
        {
            var dt = DateTime.Now;
            _fileName = @"E:\Notifierlog.txt";
        }

        public void OnCreated(FileSystemEventArgs arg)
        {
            SaveIntoFile(string.Format("[{0}]: {1}, {2}", DateTime.Now, arg.ChangeType, arg.FullPath));
        }

        public void OnRename(RenamedEventArgs arg)
        {
            SaveIntoFile(string.Format("[{0}]: {1} from {3} to {4}, {2}", DateTime.Now, arg.ChangeType, arg.FullPath, arg.OldName, arg.Name));
        }

        private void SaveIntoFile(string value)
        {
            using (var sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(value);
            }
        }
    }
}
