using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Charity.API.Mappers;
using Charity.Common.Models;
using Charity.DAL;
using Newtonsoft.Json;
using Xunit;

namespace Charity.API.Tests
{
    public class EnrollmentApiTests : IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public EnrollmentApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllEnrollments_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Enrollment");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enrollments = await response.Content.ReadFromJsonAsync<ICollection<EnrollmentListModel>>();
            Assert.NotNull(enrollments);
            Assert.NotEmpty(enrollments);
        }

        [Fact]
        public async Task GetEnrollmentById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Enrollment/{DbSeed.Enrollment.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var enrollment = await response.Content.ReadFromJsonAsync<EnrollmentListModel>();
            Assert.NotNull(enrollment);
        }

        [Fact]
        public async Task GetEnrollmentById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Enrollment/1ed7771c-efe3-4249-9bd4-b0d16b5b29c1");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetEnrollmentById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Enrollment/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateEnrollment_SuccessTest()
        {
            // Arrange
            var newEnrollment = new EnrollmentDetailModel()
            {
                DateTime = new DateTime(2012, 12, 21, 15, 21, 10),
                VolunteerEmail = DbSeed.Volunteer.Email,
                VolunteerId = DbSeed.Volunteer.Id,
                VolunteeringTitle = DbSeed.Volunteering.Title,
                VolunteeringId = DbSeed.Volunteering.Id

            };
            var enrollmentJson = JsonConvert.SerializeObject(newEnrollment);
            var data = new StringContent(enrollmentJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Enrollment", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateEnrollment_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Enrollment", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEnrollment_SuccessTest()
        {
            // Arrange
            var oldEnrollment = _mapper.Map<EnrollmentDetailModel>(DbSeed.Enrollment);
            var newEnrollment = new EnrollmentDetailModel()
            {
                Id = oldEnrollment.Id,
                DateTime = new DateTime(2011, 12, 21, 15, 21, 10),
                VolunteerId = oldEnrollment.VolunteerId,
                VolunteerEmail = oldEnrollment.VolunteerEmail,
                VolunteeringId = oldEnrollment.VolunteeringId,
                VolunteeringTitle = oldEnrollment.VolunteeringTitle
            };
            var enrollmentJson = JsonConvert.SerializeObject(newEnrollment);
            var data = new StringContent(enrollmentJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Enrollment", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newEnrollment.Id, updatedId);
        }

        [Fact]
        public async Task UpdateEnrollment_BadIdTest()
        {
            // Arrange
            var newEnrollment = new EnrollmentDetailModel()
            {
                Id = Guid.Parse("f9f4c787-a90c-4782-ab89-5edfcf307c19"),
                DateTime = new DateTime(2021, 12, 21, 15, 21, 10),
                VolunteerEmail = "volunteer@mail.com",
                VolunteerId = Guid.Parse("952f40f0-8181-4cc6-aff8-d932e002d98f"),
                VolunteeringId = Guid.Parse("82c10cd2-4e01-4738-b8b6-e9b30798985d"),
                VolunteeringTitle = "New title"
            };
            var enrollmentJson = JsonConvert.SerializeObject(newEnrollment);
            var data = new StringContent(enrollmentJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Enrollment", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEnrollment_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Enrollment", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEnrollment_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Enrollment/{DbSeed.Enrollment.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEnrollment_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Enrollment/{DbSeed.Volunteer.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
