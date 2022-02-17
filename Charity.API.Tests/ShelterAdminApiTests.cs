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
    public class ShelterAdminApiTests :IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public ShelterAdminApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllShelterAdmins_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/ShelterAdmin");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelterAdmins = await response.Content.ReadFromJsonAsync<ICollection<ShelterAdminListModel>>();
            Assert.NotNull(shelterAdmins);
            Assert.NotEmpty(shelterAdmins);
        }

        [Fact]
        public async Task GetShelterAdminById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/ShelterAdmin/{DbSeed.ShelterAdmin.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var shelterAdmin = await response.Content.ReadFromJsonAsync<ShelterAdminListModel>();
            Assert.NotNull(shelterAdmin);
        }

        [Fact]
        public async Task GetShelterAdminById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/ShelterAdmin/98114274-f0e6-40a4-8207-b5663671030d");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetShelterAdminById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/ShelterAdmin/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateShelterAdmin_SuccessTest()
        {
            // Arrange
            var newShelterAdmin = new ShelterAdminDetailModel()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@mail.com",
                Password = "newpassword",
                Phone = "420777888999",
                Role = "admin",
                PhotoURL = "Some photo"
            };
            var shelterAdminJson = JsonConvert.SerializeObject(newShelterAdmin);
            var data = new StringContent(shelterAdminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/ShelterAdmin", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateShelterAdmin_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/ShelterAdmin", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateShelterAdmin_SuccessTest()
        {
            // Arrange
            var oldShelterAdmin = _mapper.Map<ShelterAdminDetailModel>(DbSeed.ShelterAdmin);
            var newShelterAdmin = new ShelterAdminDetailModel()
            {
                Id = oldShelterAdmin.Id,
                FirstName = "New",
                LastName = "Name",
                Email = oldShelterAdmin.Email,
                Password = oldShelterAdmin.Password,
                PhotoURL = "Some photo"
            };
            var shelterAdminJson = JsonConvert.SerializeObject(newShelterAdmin);
            var data = new StringContent(shelterAdminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/ShelterAdmin", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newShelterAdmin.Id, updatedId);
        }

        [Fact]
        public async Task UpdateShelterAdmin_BadIdTest()
        {
            // Arrange
            var newShelterAdmin = new ShelterAdminDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-5a92-46ae-bfbc-54e2d58006bb"),
                Email = "newemail@mail.com",
                FirstName = "New",
                LastName = "Name",
                Password = "newpassword",
                Role = "shelter-admin"
            };
            var shelterAdminJson = JsonConvert.SerializeObject(newShelterAdmin);
            var data = new StringContent(shelterAdminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/ShelterAdmin", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateShelterAdmin_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/ShelterAdmin", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteShelterAdmin_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/ShelterAdmin/{DbSeed.ShelterAdmin.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteShelterAdmin_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/ShelterAdmin/{DbSeed.Volunteer.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
