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
    public class ShelterApiTests : IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public ShelterApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }


        [Fact]
        public async Task GetAllShelters_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Shelter");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelters = await response.Content.ReadFromJsonAsync<ICollection<ShelterListModel>>();
            Assert.NotNull(shelters);
            Assert.NotEmpty(shelters);
        }

        [Fact]
        public async Task GetShelterById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Shelter/{DbSeed.Shelter.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelter = await response.Content.ReadFromJsonAsync<ShelterListModel>();
            Assert.NotNull(shelter);
        }

        [Fact]
        public async Task GetShelterById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Shelter/df2e39d9-f691-4f9b-8533-61c2544c23f7");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetShelterById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Shelter/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateShelter_SuccessTest()
        {
            // Arrange
            var newShelter = new ShelterDetailModel()
            {
                Title = "Catty charity",
                Description = "This is a cat help charity",
                PhotoURL = "Some photo"
            };
            var shelterJson = JsonConvert.SerializeObject(newShelter);
            var data = new StringContent(shelterJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Shelter", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateShelter_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Shelter", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateShelter_SuccessTest()
        {
            // Arrange
            var oldShelter = _mapper.Map<ShelterDetailModel>(DbSeed.Shelter);
            var newShelter = new ShelterDetailModel()
            {
                Id = oldShelter.Id,
                Title = "New title",
                Description = oldShelter.Description,
                PhotoURL = "Some photo"
            };
            var shelterJson = JsonConvert.SerializeObject(newShelter);
            var data = new StringContent(shelterJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Shelter", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newShelter.Id, updatedId);
        }

        [Fact]
        public async Task UpdateShelter_BadIdTest()
        {
            // Arrange
            var newShelter = new ShelterDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                Title = "New title",
                Description = "New description"
            };
            var shelterJson = JsonConvert.SerializeObject(newShelter);
            var data = new StringContent(shelterJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Shelter", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateShelter_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Shelter", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteShelter_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Shelter/{DbSeed.Shelter.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteShelter_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Shelter/{DbSeed.Shelter.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchShelter_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Shelter/Search?search=Super");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelters = await response.Content.ReadFromJsonAsync<ICollection<ShelterListModel>>();
            Assert.NotNull(shelters);
            Assert.NotEmpty(shelters);
        }

        [Fact]
        public async Task SearchShelter_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Shelter/Search?search=ipsum");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelters = await response.Content.ReadFromJsonAsync<ICollection<ShelterListModel>>();
            Assert.NotNull(shelters);
            Assert.NotEmpty(shelters);
        }


        [Fact]
        public async Task SearchShelter_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Shelter/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchShelter_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Shelter/Search?search=No shelter");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelters = await response.Content.ReadFromJsonAsync<ICollection<ShelterListModel>>();
            Assert.NotNull(shelters);
            Assert.Empty(shelters);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
