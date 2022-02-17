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
    public class VolunteeringRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly VolunteeringRepository _dbRepositorySUT;

        public VolunteeringRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new VolunteeringRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewVolunteeringTest()
        {
            //Arrange
            var newVolunteering = new VolunteeringEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                PhotoURL = "Some photo",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            //Act
            _dbRepositorySUT.Insert(newVolunteering);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedVolunteering = dbx.Volunteerings.FirstOrDefault(entity => entity.Id == newVolunteering.Id);
            Assert.Equal(newVolunteering, retrievedVolunteering);
        }

        [Fact]
        public void GetVolunteeringTest()
        {
            //Arrange
            var existingVolunteering = new VolunteeringEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                PhotoURL = "Some photo",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteerings.Add(existingVolunteering);
            dbx.SaveChanges();

            //Act
            VolunteeringEntity retrievedVolunteering = _dbRepositorySUT.Get(existingVolunteering.Id);

            //Assert
            Assert.Equal(existingVolunteering, retrievedVolunteering);
        }

        [Fact]
        public void GetSeedeVolunteeringTest()
        {
            //Arrange
            var existingVolunteering = DbSeed.Volunteering;

            //Act
            VolunteeringEntity retrievedVolunteering = _dbRepositorySUT.Get(existingVolunteering.Id);

            //Assert
            Assert.Equal(existingVolunteering, retrievedVolunteering);
        }

        [Fact]
        public void GetAllVolunteeringsTest()
        {
            //Arrange

            var exitingVolunteerings = new List<VolunteeringEntity>()
            {
                new VolunteeringEntity()
                {
                    Id = Guid.NewGuid(),
                    Title = "Some Title 1",
                    PhotoURL = "Some photo",
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                },
                new VolunteeringEntity()
                {
                    Id = Guid.NewGuid(),
                    Title = "Some Title 2",
                    PhotoURL = "Another photo",
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteerings.AddRange(exitingVolunteerings);
            dbx.SaveChanges();

            //Act
            IEnumerable<VolunteeringEntity> retrievedVolunteerings = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingVolunteerings, retrievedVolunteerings);
        }

        [Fact]
        public void UpdateVolunteeringTest()
        {
            //Arrange
            var existingVolunteering = new VolunteeringEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteerings.Add(existingVolunteering);
            dbx.SaveChanges();

            var oldTitle = existingVolunteering.Title;

            //Act
            VolunteeringEntity updatedVolunteering = dbx.Volunteerings.Single(s => s.Id == existingVolunteering.Id);
            updatedVolunteering.Title = "New Title of Volunteering";
            _dbRepositorySUT.Update(updatedVolunteering);

            //Assert
            var retrievedVolunteering = dbx.Volunteerings.Single(Volunteering => Volunteering.Id == existingVolunteering.Id);

            Assert.Equal(updatedVolunteering.Id, retrievedVolunteering.Id);
            Assert.NotEqual(oldTitle, retrievedVolunteering.Title);
            Assert.Equal(updatedVolunteering.Shelter, retrievedVolunteering.Shelter);
            Assert.Equal(updatedVolunteering.ShelterId, retrievedVolunteering.ShelterId);

        }

        [Fact]
        public void DeleteVolunteeringTest()
        {
            //Arrange
            var existingVolunteering = new VolunteeringEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Volunteerings.Add(existingVolunteering);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingVolunteering.Id);

            //Assert
            VolunteeringEntity retrievedVolunteering = dbx.Volunteerings.FirstOrDefault(admin => admin.Id == existingVolunteering.Id);
            Assert.Null(retrievedVolunteering);
        }

        [Fact]
        public void DeleteVolunteeringFormShelterTest()
        {
            //Arrange
            var existingShelter = DbSeed.Shelter;
            var exitingVolunteering = DbSeed.Volunteering;

            //Act
            _dbRepositorySUT.Delete(exitingVolunteering.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            ShelterEntity retrievedShelter = dbx.Shelters.SingleOrDefault(shelter => shelter.Id == existingShelter.Id);
            Assert.Empty(retrievedShelter.Volunteerings);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
