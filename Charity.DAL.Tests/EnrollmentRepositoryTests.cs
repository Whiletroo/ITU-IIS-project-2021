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
    public class EnrollmentRepositoryTests : IDisposable
    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly EnrollmentRepository _dbRepositorySUT;

        public EnrollmentRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(DbContextInMemoryFactory));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _dbRepositorySUT = new EnrollmentRepository(_dbContextFactory);
        }

        [Fact]
        public void InsertNewEnrollmentTest()
        {
            //Arrange
            var newEnrollment = new EnrollmentEntity()
            {
                Id = Guid.NewGuid(),
                DateTime = new DateTime(2021, 10, 22, 12, 33, 0)
            };

            //Act
            _dbRepositorySUT.Insert(newEnrollment);

            //Assert
            using var dbx = _dbContextFactory.CreateDbContext();
            var retrievedEnrollment = dbx.Enrollments.FirstOrDefault(entity => entity.Id == newEnrollment.Id);
            Assert.Equal(newEnrollment, retrievedEnrollment);
        }

        [Fact]
        public void GetEnrollmentTest()
        {
            //Arrange
            var shelter = new ShelterEntity()
            {
                Id = Guid.NewGuid(),
                Title = "New shelter"
            };
            var volunteering = new VolunteeringEntity()
            {
                Id = Guid.NewGuid(),
                Title = "New volunteering",
                DateTime = new DateTime(2021, 10, 24, 12, 33, 0),
                Description = "Some volunteering",
                RequiredCount = 10,
                PhotoURL = "Some photo",
                Shelter = shelter,
                ShelterId = shelter.Id
            };
            var volunteer = new VolunteerEntity()
            {
                Id = Guid.NewGuid(),
                FirstName = "name",
                LastName = "surname",
                Email = "mail",
                Password = "pass",
                Transactions = new List<TransactionEntity>() {}
            };
            var existingEnrollment = new EnrollmentEntity()
            {
                Id = Guid.NewGuid(),
                DateTime = new DateTime(2021, 10, 21, 11, 52, 0),
                Volunteer = volunteer,
                VolunteerId = volunteer.Id,
                Volunteering = volunteering,
                VolunteeringId = volunteering.Id
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Shelters.Add(shelter);
            dbx.SaveChanges();
            dbx.Volunteerings.Add(volunteering);
            dbx.SaveChanges();
            dbx.Volunteers.Add(volunteer);
            dbx.SaveChanges();
            dbx.Enrollments.Add(existingEnrollment);
            dbx.SaveChanges();

            //Act
            EnrollmentEntity retrievedEnrollment = _dbRepositorySUT.Get(existingEnrollment.Id);

            //Assert
            Assert.Equal(existingEnrollment, retrievedEnrollment);
        }

        [Fact]
        public void GetSeededEnrollment()
        {
            //Arrange
            var existingEnrollment = DbSeed.Enrollment;

            //Act
            EnrollmentEntity retrievedEnrollment = _dbRepositorySUT.Get(existingEnrollment.Id);

            //Assert
            Assert.Equal(existingEnrollment, retrievedEnrollment);
        }

        [Fact]
        public void GetAllEnrollmentsTest()
        {
            //Arrange

            var exitingEnrollments = new List<EnrollmentEntity>()
            {
                new EnrollmentEntity()
                {
                    Id = Guid.NewGuid(),
                    DateTime = new DateTime(2021, 10, 22, 12, 33, 0)
                },
                new EnrollmentEntity()
                {
                    Id = Guid.NewGuid(),
                    DateTime = new DateTime(2021, 10, 21, 11, 52, 0)
                }
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Enrollments.AddRange(exitingEnrollments);
            dbx.SaveChanges();

            //Act
            IEnumerable<EnrollmentEntity> retrievedEnrollments = _dbRepositorySUT.GetAll();

            //Assert
            Assert.Equal(exitingEnrollments, retrievedEnrollments);
        }

        [Fact]
        public void UpdateEnrollmentTest()
        {
            //Arrange
            var existingEnrollment = new EnrollmentEntity()
            {
                Id = Guid.NewGuid(),
                DateTime = new DateTime(2021, 11, 23, 21, 57, 0)
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Enrollments.Add(existingEnrollment);
            dbx.SaveChanges();

            var oldDateTime = existingEnrollment.DateTime;

            //Act
            EnrollmentEntity updatedEnrollment = dbx.Enrollments.FirstOrDefault(s => s.Id == existingEnrollment.Id);
            updatedEnrollment.DateTime = new DateTime(1999, 11, 21, 22, 23, 0);
            _dbRepositorySUT.Update(updatedEnrollment);

            //Assert
            var retrievedEnrollment = dbx.Enrollments.Single(Enrollment => Enrollment.Id == existingEnrollment.Id);

            Assert.Equal(updatedEnrollment.Id, retrievedEnrollment.Id);
            Assert.NotEqual(oldDateTime, retrievedEnrollment.DateTime);
            Assert.Equal(updatedEnrollment.VolunteerId, retrievedEnrollment.VolunteerId);
            Assert.Equal(updatedEnrollment.VolunteeringId, retrievedEnrollment.VolunteeringId);
        }

        [Fact]
        public void DeleteEnrollmentTest()
        {
            //Arrange
            var existingEnrollment = new EnrollmentEntity()
            {
                Id = Guid.NewGuid(),
                DateTime = new DateTime(2021, 11, 23, 21, 57, 0),
                Volunteer = new VolunteerEntity(),
                VolunteerId = Guid.NewGuid(),
                Volunteering = new VolunteeringEntity(),
                VolunteeringId = Guid.NewGuid()
            };

            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Enrollments.Add(existingEnrollment);
            dbx.SaveChanges();

            //Act
            _dbRepositorySUT.Delete(existingEnrollment.Id);

            //Assert
            EnrollmentEntity retrievedEnrollment = dbx.Enrollments.FirstOrDefault(admin => admin.Id == existingEnrollment.Id);
            Assert.Null(retrievedEnrollment);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}
