using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Project.HR.Data.Models
{
    public class HRDbContextFactory : IDesignTimeDbContextFactory<HRDbContext>
    {
        public HRDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRDbContext>();

            // Use your connection string here
            optionsBuilder.UseSqlite("Data Source=hr_database.db");

            return new HRDbContext(optionsBuilder.Options);
        }
    }
}