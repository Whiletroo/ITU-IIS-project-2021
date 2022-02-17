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
using Charity.API.Tests;
using Charity.Common.Models;
using Charity.DAL;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json;
using Xunit;

namespace Charity.API.Tests
{
    public class VolunteeringApiTests : IAsyncDisposable
    {

        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public VolunteeringApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }

        [Fact]
        public async Task GetAllVolunteering_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Volunteering");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteering = await response.Content.ReadFromJsonAsync<ICollection<VolunteeringListModel>>();
            Assert.NotNull(volunteering);
            Assert.NotEmpty(volunteering);
        }

        [Fact]
        public async Task GetVolunteeringById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteering/{DbSeed.Volunteering.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteering = await response.Content.ReadFromJsonAsync<VolunteeringListModel>();
            Assert.NotNull(volunteering);
        }

        [Fact]
        public async Task GetVolunteeringById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteering/df2e39d9-f691-4f9b-8533-61c2544c23f7");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetVolunteeringById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Volunteering/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateVolunteering_SuccessTest()
        {
            // Arrange
            var newVolunteering = new VolunteeringDetailModel()
            {
                Title = "Volunteering Title",
                ShelterTitle = "Shelter Title",
                ShelterId = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5")
            };
            var VolunteeringTeamJson = JsonConvert.SerializeObject(newVolunteering);
            var VolunteeringData = new StringContent(VolunteeringTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Volunteering", VolunteeringData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateVolunteering_BadContentTest()
        {
            // Arrange
            var newVolunteeringData = new { Value1 = "String", Value2 = 10 };
            var VolunteeringDataJson = JsonConvert.SerializeObject(newVolunteeringData);
            var VolunteeringData = new StringContent(VolunteeringDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Volunteering", VolunteeringData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateVolunteering_SuccessTest()
        {
            // Arrange
            var oldVolunteering = _mapper.Map<VolunteeringDetailModel>(DbSeed.Volunteering);
            var newVolunteering = new VolunteeringDetailModel()
            {
                Id = oldVolunteering.Id,
                Title = "New Volunteering Title",
                ShelterTitle = oldVolunteering.ShelterTitle,
                ShelterId = oldVolunteering.ShelterId
            };
            var VolunteeringTeamJson = JsonConvert.SerializeObject(newVolunteering);
            var VolunteeringData = new StringContent(VolunteeringTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteering", VolunteeringData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newVolunteering.Id, updatedId);
        }

        [Fact]
        public async Task UpdateVolunteering_BadIdTest()
        {
            // Arrange
            var newVolunteering = new VolunteeringDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                Title = "New Volunteering Title",
                ShelterTitle = "Shelter Title",
                ShelterId = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5")
            };
            var VolunteeringTeamJson = JsonConvert.SerializeObject(newVolunteering);
            var VolunteeringData = new StringContent(VolunteeringTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteering", VolunteeringData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateVolunteering_BadContentsTest()
        {
            // Arrange
            var newVolunteeringData = new { Value1 = "String", Value2 = 10 };
            var VolunteeringDataJson = JsonConvert.SerializeObject(newVolunteeringData);
            var VolunteeringData = new StringContent(VolunteeringDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Volunteering", VolunteeringData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteVolunteering_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Volunteering/{DbSeed.Volunteering.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteVolunteering_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Volunteering/{DbSeed.Volunteer.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchVolunteering_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteering/Search?search=Volunteer");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteerings = await response.Content.ReadFromJsonAsync<ICollection<VolunteeringListModel>>();
            Assert.NotNull(volunteerings);
            Assert.NotEmpty(volunteerings);
        }

        [Fact]
        public async Task SearchVolunteering_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteering/Search?search=Super");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteerings = await response.Content.ReadFromJsonAsync<ICollection<VolunteeringListModel>>();
            Assert.NotNull(volunteerings);
            Assert.NotEmpty(volunteerings);
        }

        [Fact]
        public async Task SearchVolunteering_SuccessTest3()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteering/Search?search=Help");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteerings = await response.Content.ReadFromJsonAsync<ICollection<VolunteeringListModel>>();
            Assert.NotNull(volunteerings);
            Assert.NotEmpty(volunteerings);
        }

        [Fact]
        public async Task SearchVolunteering_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteering/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchVolunteering_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Volunteering/Search?search=No volunteering");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var volunteerings = await response.Content.ReadFromJsonAsync<ICollection<VolunteeringListModel>>();
            Assert.NotNull(volunteerings);
            Assert.Empty(volunteerings);
        }
    }
}
