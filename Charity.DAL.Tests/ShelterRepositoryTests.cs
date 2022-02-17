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
    public class ShelterRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly ShelterRepository _dbRepositorySUT;

        public ShelterRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new ShelterRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewShelterTest()
        {
            //Arrange
            var newShelter = new ShelterEntity()
            {
                Id = Guid.NewGuid(),
                Description = "Good Shelter Description",
                Title = "Good Shelter",
                PhotoURL = "Some photo"
            };

            //Act
            _dbRepositorySUT.Insert(newShelter);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedShelter = dbx.Shelters.Single(entity => entity.Id == newShelter.Id);
            Assert.Equal(newShelter, retrievedShelter);
        }

        [Fact]
        public void GetShelterTest()
        {
            //Arrange
            var existingShelter = new ShelterEntity()
            {
                Id = Guid.NewGuid(),
                Description = "Good Shelter Description",
                Title = "Good Shelter",
                PhotoURL = "Some photo",
                Admin = new ShelterAdminEntity(),
                AdminId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.Add(existingShelter);
            dbx.SaveChanges();

            //Act
            ShelterEntity retrievedShelter = _dbRepositorySUT.Get(existingShelter.Id);

            //Assert
            Assert.Equal(existingShelter, retrievedShelter);
        }

        [Fact]
        public void GetSeedeShelterTest()
        {
            //Arrange
            var existingShelter = DbSeed.Shelter;

            //Act
            ShelterEntity retrievedShelter = _dbRepositorySUT.Get(existingShelter.Id);

            //Assert
            Assert.Equal(existingShelter, retrievedShelter);
        }

        [Fact]
        public void GetAllSheltersTest()
        {
            //Arrange

            var exitingShelters = new List<ShelterEntity>()
            {
                new ShelterEntity()
                {
                    Id = Guid.NewGuid(),
                    Description = "Good Shelter Description 1",
                    Title = "Good Shelter 1",
                    PhotoURL = "Some photo",
                    Admin = new ShelterAdminEntity(),
                    AdminId = Guid.NewGuid()
                },
                new ShelterEntity()
                {
                    Id = Guid.NewGuid(),
                    Description = "Good Shelter Description 2",
                    Title = "Good Shelter 2",
                    PhotoURL = "Another photo",
                    Admin = new ShelterAdminEntity(),
                    AdminId = Guid.NewGuid()
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.AddRange(exitingShelters);
            dbx.SaveChanges();

            //Act
            IEnumerable<ShelterEntity> retrievedShelters = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingShelters, retrievedShelters);
        }

        [Fact]
        public void UpdateShelterTest()
        {
            //Arrange
            var existingShelter = new ShelterEntity()
            {
                Id = Guid.Parse("554059f1-3cae-472b-85ce-3e7bf2f6e58e"),
                Description = "Good Shelter Description",
                Title = "Good Shelter"
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.Add(existingShelter);
            dbx.SaveChanges();

            var oldDescription = existingShelter.Description;

            //Act
            var updatedShelter = dbx.Shelters.Single(s => s.Id == existingShelter.Id);
            updatedShelter.Description = "Another Shelter Description";
            _dbRepositorySUT.Update(updatedShelter);

            //Assert
            var retrievedShelter = dbx.Shelters.Single(shelter => shelter.Id == updatedShelter.Id);
            Assert.Equal(updatedShelter.Id, retrievedShelter.Id);
            Assert.Equal(updatedShelter.Title, retrievedShelter.Title);
            Assert.Equal(updatedShelter.Description, retrievedShelter.Description);
            Assert.NotEqual(oldDescription, retrievedShelter.Description);
        }

        [Fact]
        public void DeleteShelterTest()
        {
            //Arrange
            var existingShelter = new ShelterEntity()
            {
                Id = Guid.NewGuid(),
                Description = "Good Shelter Description",
                Title = "Good Shelter"
            };
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.Add(existingShelter);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingShelter.Id);

            //Assert
            ShelterEntity retrievedShelter = dbx.Shelters.SingleOrDefault(shelter => shelter.Id == existingShelter.Id);
            Assert.Null(retrievedShelter);
        }

        [Fact]
        public void DeleteSeededShelterTest()
        {
            //Arrange
            var existingShelter = DbSeed.Shelter;

            //Act
            _dbRepositorySUT.Delete(existingShelter.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            ShelterEntity retrievedShelter = dbx.Shelters.SingleOrDefault(shelter => shelter.Id == existingShelter.Id);
            Assert.Null(retrievedShelter);
        }

        [Fact]
        public void DeleteShelterCascadeTest()
        {
            //Arrange
            var existingShelter = DbSeed.Shelter;
            var existingDonation = DbSeed.Donation;
            var existingVolunteering = DbSeed.Volunteering;

            //Act
            _dbRepositorySUT.Delete(existingShelter.Id);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            ShelterEntity retrievedShelter = dbx.Shelters.SingleOrDefault(shelter => shelter.Id == existingShelter.Id);
            DonationEntity retrievedDonation = dbx.Donations.SingleOrDefault(donation => donation.Id == existingDonation.Id);
            VolunteeringEntity retrievedVolunteering = dbx.Volunteerings.SingleOrDefault(volunteering => volunteering.Id == existingVolunteering.Id);
            Assert.Null(retrievedShelter);
            Assert.Null(retrievedDonation);
            Assert.Null(retrievedVolunteering);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
