using System;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL.Tests
{
    class DbContextInMemoryFactory : IDbContextFactory<CharityDbContext>
    {
        private readonly string _databaseName;
        public DbContextInMemoryFactory(string databaseName)
        {
            _databaseName = databaseName;
        }
        public CharityDbContext CreateDbContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<CharityDbContext>();
            contextOptionsBuilder.UseInMemoryDatabase(_databaseName);
            return new CharityDbContext(contextOptionsBuilder.Options);
        }
    }
}