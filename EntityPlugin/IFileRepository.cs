using System.Collections.Generic;

namespace EntityPlugin
{
    public interface IFileRepository : IRepository<File>
    {
        IEnumerable<File> GetFiles();
    }
}