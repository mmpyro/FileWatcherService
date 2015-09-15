using System;
using System.Diagnostics;
using System.IO;
using FileNotifier;

namespace EntityPlugin
{
    public class FileEntity : IFileNotifier
    {
        public void OnCreated(FileSystemEventArgs arg)
        {
            try
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork(new FileModelContext()))
                {
                    var fileRepository = unitOfWork.CreateFileRepository();
                    fileRepository.Add(new File()
                    {
                        ModifiedDateTime = DateTime.Now,
                        ChangeType = arg.ChangeType.ToString(),
                        FullPath = arg.FullPath
                    });
                    unitOfWork.SaveChanges();
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void OnRename(RenamedEventArgs arg)
        {
            try
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork(new FileModelContext()))
                {
                    var fileRepository = unitOfWork.CreateFileRepository();
                    fileRepository.Add(new File()
                    {
                        ModifiedDateTime = DateTime.Now,
                        ChangeType = arg.ChangeType.ToString(),
                        FullPath = arg.FullPath,
                        NewName = arg.Name,
                        OldName = arg.OldName
                    });
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}