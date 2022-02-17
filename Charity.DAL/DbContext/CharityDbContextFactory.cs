using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Charity.DAL
{
    public class CharityDbContextFactory : IDbContextFactory<CharityDbContext>
    {
        private readonly IConfiguration _config;

        public CharityDbContextFactory(IConfiguration config) : base()
        {
            _config = config;
        }

        public CharityDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CharityDbContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            return new CharityDbContext(optionsBuilder.Options);
        }
    }
}
