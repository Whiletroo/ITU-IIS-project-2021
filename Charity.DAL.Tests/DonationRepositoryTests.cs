//< !--Author xpimen00-- >
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
    public class DonationRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly DonationRepository _dbRepositorySUT;

        public DonationRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new DonationRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewDonationTest()
        {
            //Arrange
            var newDonation = new DonationEntity()
            {
                Id = Guid.NewGuid(),
                Title =  "Some Title",
                PhotoURL = "Some photo",
                Goal = 1000000,
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            //Act
            _dbRepositorySUT.Insert(newDonation);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedDonation = dbx.Donations.FirstOrDefault(entity => entity.Id == newDonation.Id);
            Assert.Equal(newDonation, retrievedDonation);
        }

        [Fact]
        public void GetDonationTest()
        {
            //Arrange
            var existingDonation = new DonationEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                PhotoURL = "Some photo",
                Goal = 1000000,
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Donations.Add(existingDonation);
            dbx.SaveChanges();

            //Act
            DonationEntity retrievedDonation = _dbRepositorySUT.Get(existingDonation.Id);

            //Assert
            Assert.Equal(existingDonation, retrievedDonation);
        }

        [Fact]
        public void GetSeededDonationTest()
        {
            //Arrange
            var existingDonation = DbSeed.Donation;

            //Act
            DonationEntity retrievedDonation = _dbRepositorySUT.Get(existingDonation.Id);

            //Assert
            Assert.Equal(existingDonation, retrievedDonation);
        }

        [Fact]
        public void GetAllDonationsTest()
        {
            //Arrange

            var exitingDonations = new List<DonationEntity>()
            {
                new DonationEntity()
                {
                    Id = Guid.NewGuid(),
                    Title =  "Some Title 1",
                    PhotoURL = "Some photo",
                    Goal = 1000000,
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                },
                new DonationEntity()
                {
                    Id = Guid.NewGuid(),
                    Title =  "Some Title 2",
                    PhotoURL = "Another photo",
                    Goal = 2312987,
                    Shelter = new ShelterEntity(),
                    ShelterId = Guid.NewGuid()
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Donations.AddRange(exitingDonations);
            dbx.SaveChanges();

            //Act
            IEnumerable<DonationEntity> retrievedDonations = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingDonations, retrievedDonations);
        }

        [Fact]
        public void UpdateDonationTest()
        {
            //Arrange
            var existingDonation = new DonationEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                Goal = 1000000,
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Donations.Add(existingDonation);
            dbx.SaveChanges();

            var oldTitle = existingDonation.Title;

            //Act
            DonationEntity updatedDonation = dbx.Donations.Single(s => s.Id == existingDonation.Id);
            updatedDonation.Title = "New Title of donation";
            _dbRepositorySUT.Update(updatedDonation);

            //Assert
            var retrievedDonation = dbx.Donations.Single(Donation => Donation.Id == existingDonation.Id);

            Assert.Equal(updatedDonation.Id, retrievedDonation.Id);
            Assert.NotEqual(oldTitle, retrievedDonation.Title);
            Assert.Equal(updatedDonation.Goal, retrievedDonation.Goal);
            Assert.Equal(updatedDonation.Shelter, retrievedDonation.Shelter);
            Assert.Equal(updatedDonation.ShelterId, retrievedDonation.ShelterId);

        }

        [Fact]
        public void DeleteDonationTest()
        {
            //Arrange
            var existingDonation = new DonationEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Some Title",
                Goal = 1000000,
                Shelter = new ShelterEntity(),
                ShelterId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Donations.Add(existingDonation);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingDonation.Id);

            //Assert
            DonationEntity retrievedDonation = dbx.Donations.FirstOrDefault(admin => admin.Id == existingDonation.Id);
            Assert.Null(retrievedDonation);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
