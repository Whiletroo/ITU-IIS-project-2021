using System;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL
{
    public class CharityDbContextDevFactory : IDbContextFactory<CharityDbContext>
    {
        public CharityDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CharityDbContext>();
            optionsBuilder.UseInMemoryDatabase("TestDb");

            var dbContext = new CharityDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
