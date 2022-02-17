//<!-- Author xpimen00-->
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
    public class VolunteerRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly VolunteerRepository _dbRepositorySUT;

        public VolunteerRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new VolunteerRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewVolunteerTest()
        {
            //Arrange
            var newVolunteer = new VolunteerEntity()
            {
                Id = Guid.NewGuid(),
                Email = "volunteer@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password",
            };

            //Act
            _dbRepositorySUT.Insert(newVolunteer);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedVolunteer = dbx.Volunteers.FirstOrDefault(entity => entity.Id == newVolunteer.Id);
            Assert.Equal(newVolunteer, retrievedVolunteer);
        }

        [Fact]
        public void GetVolunteerTest()
        {
            //Arrange
            var existingVolunteer = new VolunteerEntity()
            {
                Id = Guid.NewGuid(),
                Email = "volunteer@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteers.Add(existingVolunteer);
            dbx.SaveChanges();

            //Act
            VolunteerEntity retrievedVolunteer = _dbRepositorySUT.Get(existingVolunteer.Id);

            //Assert
            Assert.Equal(existingVolunteer, retrievedVolunteer);
        }

        [Fact]
        public void GetSeededVolunteerTest()
        {
            //Arrange
            var existingVolunteer = DbSeed.Volunteer;

            //Act
            VolunteerEntity retrievedVolunteer = _dbRepositorySUT.Get(existingVolunteer.Id);

            //Assert
            Assert.Equal(existingVolunteer, retrievedVolunteer);
        }

        [Fact]
        public void GetAllVolunteersTest()
        {
            //Arrange

            var exitingVolunteers = new List<VolunteerEntity>()
            {
                new VolunteerEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "volunteer1@mail",
                    FirstName = "Name1",
                    LastName = "Surname1",
                    Password = "password1",
                },
                new VolunteerEntity()
                {
                    Id = Guid.NewGuid(),
                    Email = "volunteer321321_#33@mail",
                    FirstName = "Name2",
                    LastName = "Surname2",
                    Password = "password2"
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteers.AddRange(exitingVolunteers);
            dbx.SaveChanges();

            //Act
            IEnumerable<VolunteerEntity> retrievedVolunteers = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingVolunteers, retrievedVolunteers);
        }

        [Fact]
        public void UpdateVolunteerTest()
        {
            //Arrange
            var existingVolunteer = new VolunteerEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteers.Add(existingVolunteer);
            dbx.SaveChanges();

            var oldFirstName = existingVolunteer.FirstName;
            var oldLastName = existingVolunteer.LastName;

            //Act
            VolunteerEntity updatedVolunteer = dbx.Volunteers.Single(s => s.Id == existingVolunteer.Id);
            updatedVolunteer.FirstName = "New Name";
            updatedVolunteer.LastName = "New Surname";
            _dbRepositorySUT.Update(updatedVolunteer);

            //Assert
            var retrievedVolunteer = dbx.Volunteers.Single(Volunteer => Volunteer.Id == existingVolunteer.Id);

            Assert.Equal(updatedVolunteer.Id, retrievedVolunteer.Id);
            Assert.Equal(updatedVolunteer.Email, retrievedVolunteer.Email);
            Assert.Equal(updatedVolunteer.FirstName, retrievedVolunteer.FirstName);
            Assert.NotEqual(oldFirstName, retrievedVolunteer.FirstName);
            Assert.Equal(updatedVolunteer.LastName, retrievedVolunteer.LastName);
            Assert.NotEqual(oldLastName, retrievedVolunteer.LastName);
            Assert.Equal(updatedVolunteer.Password, retrievedVolunteer.Password);
        }

        [Fact]
        public void DeleteVolunteerTest()
        {
            //Arrange
            var existingVolunteer = new VolunteerEntity()
            {
                Id = Guid.NewGuid(),
                Email = "admin@mail",
                FirstName = "Name",
                LastName = "Surname",
                Password = "password"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteers.Add(existingVolunteer);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingVolunteer.Id);

            //Assert
            VolunteerEntity retrievedVolunteer = dbx.Volunteers.FirstOrDefault(admin => admin.Id == existingVolunteer.Id);
            Assert.Null(retrievedVolunteer);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
