using System.Collections.Generic;
using System.Data.Entity;

namespace EntityPlugin
{
    public class FileRepository : IFileRepository
    {
        private readonly FileModelContext _context;

        public FileRepository(FileModelContext context)
        {
            _context = context;
        }

        public void Add(File item)
        {
            _context.Files.Add(item);
        }

        public void Delete(File item)
        {
            _context.Files.Remove(item);
        }

        public void Update(File item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<File> GetFiles()
        {
            return _context.Files;
        }
    }
}