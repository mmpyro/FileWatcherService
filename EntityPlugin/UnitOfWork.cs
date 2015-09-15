namespace EntityPlugin
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileModelContext _context;

        public UnitOfWork(FileModelContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IFileRepository CreateFileRepository()
        {
            return new FileRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}