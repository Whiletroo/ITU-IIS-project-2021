//<!-- Author xpimen00-->
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
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using Xunit;

namespace Charity.API.Tests
{
    public class VolunteerApiTests : IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public VolunteerApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }


        [Fact]
        public async Task GetAllVolunteer_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Volunteer");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteers = await response.Content.ReadFromJsonAsync<ICollection<VolunteerListModel>>();
            Assert.NotNull(volunteers);
            Assert.NotEmpty(volunteers);
        }

        [Fact]
        public async Task GetVolunteerById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteer/{DbSeed.Volunteer.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteer = await response.Content.ReadFromJsonAsync<VolunteerListModel>();
            Assert.NotNull(volunteer);
        }

        [Fact]
        public async Task GetVolunteerById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteer/df2e39d9-f691-4f9b-8533-61c2544c23f7");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetVolunteerById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteer/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateVolunteer_SuccessTest()
        {
            // Arrange
            var newVolunteer = new VolunteerDetailModel()
            {
                FirstName = "SomeFirstName",
                LastName = "SomeLastName",
                Email = "SomeVolunteer@mail.com",
                Password = "SomePassWord"
            };
            var VolunteerTeamJson = JsonConvert.SerializeObject(newVolunteer);
            var VolunteerData = new StringContent(VolunteerTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Volunteer", VolunteerData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateVolunteer_BadContentTest()
        {
            // Arrange
            var newVolunteerData = new { Value1 = "String", Value2 = 10 };
            var dataVolunteerJson = JsonConvert.SerializeObject(newVolunteerData);
            var VolunteerData = new StringContent(dataVolunteerJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Volunteer", VolunteerData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateVolunteer_SuccessTest()
        {
            // Arrange
            var oldVolunteer = _mapper.Map<VolunteerDetailModel>(DbSeed.Volunteer);
            var newVolunteer = new VolunteerDetailModel()
            {
                Id = oldVolunteer.Id,
                FirstName = "New SomeFirstName",
                LastName = "New SomeLastName",
                Email = "New SomeVolunteer@mail.com",
                Password = "New SomePassWord"
            };
            var VolunteerTeamJson = JsonConvert.SerializeObject(newVolunteer);
            var VolunteerData = new StringContent(VolunteerTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteer", VolunteerData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newVolunteer.Id, updatedId);
        }

        [Fact]
        public async Task UpdateVolunteer_BadIdTest()
        {
            // Arrange
            var newVolunteer = new VolunteerDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                FirstName = "New SomeFirstName",
                LastName = "New SomeLastName",
                Email = "New SomeVolunteer@mail.com",
                Password = "New SomePassWord"
            };
            var VolunteerTeamJson = JsonConvert.SerializeObject(newVolunteer);
            var VolunteerData = new StringContent(VolunteerTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteer", VolunteerData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateVolunteer_BadContentsTest()
        {
            // Arrange
            var newVolunteerData = new { Value1 = "String", Value2 = 10 };
            var VolunteerDataJson = JsonConvert.SerializeObject(newVolunteerData);
            var VolunteerData = new StringContent(VolunteerDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteer", VolunteerData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteVolunteer_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Volunteer/{DbSeed.Volunteer.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteVolunteer_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Volunteer/{DbSeed.Transaction.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchVolunteer_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteer/Search?search=Jane");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteers = await response.Content.ReadFromJsonAsync<ICollection<VolunteerListModel>>();
            Assert.NotNull(volunteers);
            Assert.NotEmpty(volunteers);
        }

        [Fact]
        public async Task SearchVolunteer_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteer/Search?search=Sue");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteers = await response.Content.ReadFromJsonAsync<ICollection<VolunteerListModel>>();
            Assert.NotNull(volunteers);
            Assert.NotEmpty(volunteers);
        }

        [Fact]
        public async Task SearchVolunteer_SuccessTest3()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteer/Search?search=janedoe@mail.com");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteers = await response.Content.ReadFromJsonAsync<ICollection<VolunteerListModel>>();
            Assert.NotNull(volunteers);
            Assert.NotEmpty(volunteers);
        }

        [Fact]
        public async Task SearchVolunteer_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteer/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchVolunteer_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteer/Search?search=No volunteer");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteers = await response.Content.ReadFromJsonAsync<ICollection<VolunteerListModel>>();
            Assert.NotNull(volunteers);
            Assert.Empty(volunteers);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
