namespace EntityPlugin
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    
    public partial class FileModelContext : DbContext
    {
        public FileModelContext()
            : base("FileModel")
        {
        }


        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FileModelContext>());
        }
    }
}
