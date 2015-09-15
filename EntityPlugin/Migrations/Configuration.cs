using System.IO;

namespace EntityPlugin.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<FileModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FileModelContext context)
        {

            IUnitOfWork unitOfWork = new UnitOfWork(context);
            var fileRepository = unitOfWork.CreateFileRepository();
            fileRepository.Add(new File()
            {
                ModifiedDateTime = DateTime.Now,
                FullPath = Path.GetTempPath(),
                ChangeType = "test"
            });
            unitOfWork.SaveChanges();
        }
    }
}
