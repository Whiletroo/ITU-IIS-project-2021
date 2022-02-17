using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Xunit;

namespace Charity.DAL.Tests
{
    public class ShelterAdminRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly ShelterAdminRepository _dbRepositorySUT;

        public ShelterAdminRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new ShelterAdminRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewShelterAdminTest()
        {
            //Arrange
            var newShelterAdmin = new ShelterAdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            //Act
            _dbRepositorySUT.Insert(newShelterAdmin);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedShelterAdmin = dbx.ShelterAdmins.Single(entity => entity.Id == newShelterAdmin.Id);
            Assert.Equal(newShelterAdmin, retrievedShelterAdmin);
        }

        [Fact]
        public void GetShelterAdminTest()
        {
            //Arrange
            var existingShelterAdmin = new ShelterAdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.ShelterAdmins.Add(existingShelterAdmin);
            dbx.SaveChanges();

            //Act
            ShelterAdminEntity retrievedShelterAdmin = _dbRepositorySUT.Get(existingShelterAdmin.Id);

            //Assert
            Assert.Equal(existingShelterAdmin, retrievedShelterAdmin);
        }

        [Fact]
        public void GetSeededShelterAdminTest()
        {
            //Arrange
            var existingShelterAdmin = DbSeed.ShelterAdmin;

            //Act
            ShelterAdminEntity retrievedShelterAdmin = _dbRepositorySUT.Get(existingShelterAdmin.Id);

            //Assert
            Assert.Equal(existingShelterAdmin, retrievedShelterAdmin);
        }

        [Fact]
        public void GetAllShelterAdminsTest()
        {
            //Arrange

            var exitingShelterAdmins = new List<ShelterAdminEntity>()
            {
                new ShelterAdminEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin1@mail",
                    FirstName = "Name1",
                    LastName = "Surname1",
                    Password = "password1",
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                },
                new ShelterAdminEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin2@mail",
                    FirstName = "Name2",
                    LastName = "Surname2",
                    Password = "password2",
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.ShelterAdmins.AddRange(exitingShelterAdmins);
            dbx.SaveChanges();

            //Act
            IEnumerable<ShelterAdminEntity> retrievedShelterAdmins = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingShelterAdmins, retrievedShelterAdmins);
        }

        [Fact]
        public void UpdateShelterAdminTest()
        {
            //Arrange
            var existingShelterAdmin = new ShelterAdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.ShelterAdmins.Add(existingShelterAdmin);
            dbx.SaveChanges();

            var oldFirstName = existingShelterAdmin.FirstName;
            var oldLastName = existingShelterAdmin.LastName;

            //Act
            ShelterAdminEntity updatedShelterAdmin = dbx.ShelterAdmins.Single(s => s.Id == existingShelterAdmin.Id);
            updatedShelterAdmin.FirstName = "New Name";
            updatedShelterAdmin.LastName = "New Seurname";
            _dbRepositorySUT.Update(updatedShelterAdmin);

            //Assert
            var retrievedShelterAdmin = dbx.ShelterAdmins.Single(shelterAdmin => shelterAdmin.Id == existingShelterAdmin.Id);

            Assert.Equal(updatedShelterAdmin.Id, retrievedShelterAdmin.Id);
            Assert.Equal(updatedShelterAdmin.Email, retrievedShelterAdmin.Email);
            Assert.Equal(updatedShelterAdmin.FirstName, retrievedShelterAdmin.FirstName);
            Assert.NotEqual(oldFirstName, retrievedShelterAdmin.FirstName);
            Assert.Equal(updatedShelterAdmin.LastName, retrievedShelterAdmin.LastName);
            Assert.NotEqual(oldLastName, retrievedShelterAdmin.LastName);
            Assert.Equal(updatedShelterAdmin.Password, retrievedShelterAdmin.Password);
            Assert.Equal(updatedShelterAdmin.Shelter, retrievedShelterAdmin.Shelter);
            Assert.Equal(updatedShelterAdmin.ShelterId, retrievedShelterAdmin.ShelterId);   
        }

        [Fact]
        public void DeleteShelterAdminTest()
        {
            //Arrange
            var existingShelterAdmin = new ShelterAdminEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.ShelterAdmins.Add(existingShelterAdmin);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingShelterAdmin.Id);

            //Assert
            ShelterAdminEntity retrievedShelterAdmin = dbx.ShelterAdmins.SingleOrDefault(shelterAdmin => shelterAdmin.Id == existingShelterAdmin.Id);
            Assert.Null(retrievedShelterAdmin);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
