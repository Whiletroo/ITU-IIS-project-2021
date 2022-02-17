//!--Author xkrukh00-- >
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Charity.DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CharityDbContext>
    {
        public CharityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CharityDbContext>();
            string solutiondir = Directory.GetParent(
                Directory.GetCurrentDirectory()).FullName;
            var configuration = new ConfigurationBuilder().SetBasePath(solutiondir + "\\" + "Charity.API" + "\\").AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new CharityDbContext(optionsBuilder.Options);
        }
    }
}