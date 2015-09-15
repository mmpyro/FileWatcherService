using System;

namespace EntityPlugin
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        IFileRepository CreateFileRepository();
    }
}