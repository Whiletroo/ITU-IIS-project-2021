using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Xunit;

namespace Charity.DAL.Tests
{
    public class AdminRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly AdminRepository _dbRepositorySUT;

        public AdminRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new AdminRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewAdminTest()
        {
            //Arrange
            var newAdmin = new AdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            //Act
            _dbRepositorySUT.Insert(newAdmin);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedAdmin = dbx.Admins.FirstOrDefault(entity => entity.Id == newAdmin.Id);
            Assert.Equal(newAdmin, retrievedAdmin);
        }

        [Fact]
        public void GetAdminTest()
        {
            //Arrange
            var existingAdmin = new AdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Admins.Add(existingAdmin);
            dbx.SaveChanges();

            //Act
            AdminEntity retrievedAdmin = _dbRepositorySUT.Get(existingAdmin.Id);

            //Assert
            Assert.Equal(existingAdmin, retrievedAdmin);
        }

        [Fact]
        public void GetSeededAdminTest()
        {
            //Arrange
            var existingAdmin = DbSeed.Admin;

            //Act
            AdminEntity retrievedAdmin = _dbRepositorySUT.Get(existingAdmin.Id);

            //Assert
            Assert.Equal(existingAdmin, retrievedAdmin);
        }

        [Fact]
        public void GetAllAdminsTest()
        {
            //Arrange

            var exitingAdmins = new List<AdminEntity>()
            {
                new AdminEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin1@mail",
                    FirstName = "Name1",
                    LastName = "Surname1",
                    Password = "password1"
                },
                new AdminEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin2@mail",
                    FirstName = "Name2",
                    LastName = "Surname2",
                    Password = "password2"
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Admins.AddRange(exitingAdmins);
            dbx.SaveChanges();

            //Act
            IEnumerable<AdminEntity> retrievedAdmins = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingAdmins, retrievedAdmins);
        }

        [Fact]
        public void UpdateAdminTest()
        {
            //Arrange
            var existingAdmin = new AdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Admins.Add(existingAdmin);
            dbx.SaveChanges();

            var oldFirstName = existingAdmin.FirstName;
            var oldLastName = existingAdmin.LastName;

            //Act
            AdminEntity updatedAdmin = dbx.Admins.FirstOrDefault(s => s.Id == existingAdmin.Id);
            updatedAdmin.FirstName = "New Name";
            updatedAdmin.LastName = "New Surname";
            _dbRepositorySUT.Update(updatedAdmin);

            //Assert
            var retrievedAdmin = dbx.Admins.Single(Admin => Admin.Id == existingAdmin.Id);

            Assert.Equal(updatedAdmin.Id, retrievedAdmin.Id);
            Assert.Equal(updatedAdmin.Email, retrievedAdmin.Email);
            Assert.Equal(updatedAdmin.FirstName, retrievedAdmin.FirstName);
            Assert.NotEqual(oldFirstName, retrievedAdmin.FirstName);
            Assert.Equal(updatedAdmin.LastName, retrievedAdmin.LastName);
            Assert.NotEqual(oldLastName, retrievedAdmin.LastName);
            Assert.Equal(updatedAdmin.Password, retrievedAdmin.Password);
        }

        [Fact]
        public void DeleteAdminTest()
        {
            //Arrange
            var existingAdmin = new AdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Admins.Add(existingAdmin);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingAdmin.Id);

            //Assert
            AdminEntity retrievedAdmin = dbx.Admins.FirstOrDefault(admin => admin.Id == existingAdmin.Id);
            Assert.Null(retrievedAdmin);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
