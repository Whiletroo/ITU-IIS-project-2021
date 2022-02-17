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
    public class TransactionRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly TransactionRepository _dbRepositorySUT;

        public TransactionRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new TransactionRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewTransactionTest()
        {
            //Arrange
            var newTransaction = new TransactionEntity()
            {
                Id = Guid.NewGuid(),
                Sum = 100,
                DateTime = new DateTime(2021,11,23,21,57,0)
            };

            //Act
            _dbRepositorySUT.Insert(newTransaction);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedTransaction = dbx.Transactions.FirstOrDefault(entity => entity.Id == newTransaction.Id);
            Assert.Equal(newTransaction, retrievedTransaction);
        }

        [Fact]
        public void GetTransactionTest()
        {
            //Arrange
            var shelter = new ShelterEntity()
            {
                Id = Guid.NewGuid(),
                Title = "New shelter"
            };
            var donation = new DonationEntity()
            {
                Id = Guid.NewGuid(),
                Goal = 100000,
                Title = "New Donation",
                Shelter = shelter,
                ShelterId = shelter.Id
            };
            var volunteer = new VolunteerEntity() { 
                Id = Guid.NewGuid(),
                FirstName = "name",
                LastName = "surname",
                Email = "mail",
                Password = "pass",
                Enrollments = new List<EnrollmentEntity>() {}
            };
            var existingTransaction = new TransactionEntity()
            {
                Id = Guid.NewGuid(),
                Sum = 100,
                DateTime = new DateTime(2021, 11, 23, 21, 57, 0),
                Volunteer = volunteer,
                VolunteerId = volunteer.Id,
                Donation = donation,
                DonationId = donation.Id
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.Add(shelter);
            dbx.SaveChanges();
            dbx.Donations.Add(donation);
            dbx.SaveChanges();
            dbx.Volunteers.Add(volunteer);
            dbx.SaveChanges();
            dbx.Transactions.Add(existingTransaction);
            dbx.SaveChanges();

            //Act
            TransactionEntity retrievedTransaction = _dbRepositorySUT.Get(existingTransaction.Id);

            //Assert
            Assert.Equal(existingTransaction, retrievedTransaction);
        }

        [Fact]
        public void GetSeededTransactionTest()
        {
            //Arrange
            var existingTransaction = DbSeed.Transaction;

            //Act
            TransactionEntity retrievedTransaction = _dbRepositorySUT.Get(existingTransaction.Id);

            //Assert
            Assert.Equal(existingTransaction, retrievedTransaction);
        }

        [Fact]
        public void GetAllTransactionsTest()
        {
            //Arrange

            var exitingTransactions = new List<TransactionEntity>()
            {
                new TransactionEntity()
                {
                    Id = Guid.NewGuid(),
                    Sum = 100,
                    DateTime = new DateTime(2021,11,23,21,57,0)
                },
                new TransactionEntity()
                {
                    Id = Guid.NewGuid(),
                    Sum = 100,
                    DateTime = new DateTime(2021,10,11,9,4,2)
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Transactions.AddRange(exitingTransactions);
            dbx.SaveChanges();

            //Act
            IEnumerable<TransactionEntity> retrievedTransactions = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingTransactions, retrievedTransactions);
        }

        [Fact]
        public void UpdateTransactionTest()
        {
            //Arrange
            var existingTransaction = new TransactionEntity()
            {
                Id = Guid.NewGuid(),
                Sum = 100,
                DateTime = new DateTime(2021, 11, 23, 21, 57, 0)
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Transactions.Add(existingTransaction);
            dbx.SaveChanges();

            var oldDateTime = existingTransaction.DateTime;

            //Act
            TransactionEntity updatedTransaction = dbx.Transactions.FirstOrDefault(s => s.Id == existingTransaction.Id);
            updatedTransaction.DateTime = new DateTime(1999, 11, 23, 21, 57, 0);
            _dbRepositorySUT.Update(updatedTransaction);

            //Assert
            var retrievedTransaction = dbx.Transactions.Single(Transaction => Transaction.Id == existingTransaction.Id);

            Assert.Equal(updatedTransaction.Id, retrievedTransaction.Id);
            Assert.Equal(updatedTransaction.Sum, retrievedTransaction.Sum);
            Assert.NotEqual(oldDateTime, retrievedTransaction.DateTime);
            Assert.Equal(updatedTransaction.VolunteerId, retrievedTransaction.VolunteerId);
            Assert.Equal(updatedTransaction.DonationId, retrievedTransaction.DonationId);
        }

        [Fact]
        public void DeleteTransactionTest()
        {
            //Arrange
            var existingTransaction = new TransactionEntity()
            {
                Id = Guid.NewGuid(),
                Sum = 100,
                DateTime = new DateTime(2021, 11, 23, 21, 57, 0),
                Volunteer = new VolunteerEntity(),
                VolunteerId = Guid.NewGuid(),
                Donation = new DonationEntity(),
                DonationId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Transactions.Add(existingTransaction);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingTransaction.Id);

            //Assert
            TransactionEntity retrievedTransaction = dbx.Transactions.FirstOrDefault(admin => admin.Id == existingTransaction.Id);
            Assert.Null(retrievedTransaction);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
