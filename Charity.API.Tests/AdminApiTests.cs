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
    public class AdminApiTests :  IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public AdminApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllAdmins_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Admin");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var admins = await response.Content.ReadFromJsonAsync<ICollection<AdminListModel>>();
            Assert.NotNull(admins);
            Assert.NotEmpty(admins);
        }

        [Fact]
        public async Task GetAdminById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Admin/{DbSeed.Admin.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var admin = await response.Content.ReadFromJsonAsync<AdminListModel>();
            Assert.NotNull(admin);
        }

        [Fact]
        public async Task GetAdminById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Admin/ba9ea829-6416-4d7b-8499-792f533bcd3d");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAdminById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Admin/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateAdmin_SuccessTest()
        {
            // Arrange
            var newAdmin = new AdminDetailModel()
            {
                FirstName = "Admin",
                LastName = "Name",
                Email = "adminname@mail.com",
                Password = "somepassword",
                PhotoURL = "Some photo",
                Role = "admin"
            };
            var adminJson = JsonConvert.SerializeObject(newAdmin);
            var data = new StringContent(adminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Admin", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateAdmin_BadContentTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Admin", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAdmin_SuccessTest()
        {
            // Arrange
            var oldAdmin = _mapper.Map<AdminDetailModel>(DbSeed.Admin);
            var newAdmin = new AdminDetailModel()
            {
                Id = oldAdmin.Id,
                FirstName = "New",
                LastName = "Name",
                Email = oldAdmin.Email,
                Password = oldAdmin.Password,
                PhotoURL = "Some photo"
            };
            var adminJson = JsonConvert.SerializeObject(newAdmin);
            var data = new StringContent(adminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Admin", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newAdmin.Id, updatedId);
        }

        [Fact]
        public async Task UpdateAdmin_BadIdTest()
        {
            // Arrange
            var newAdmin = new AdminDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-5a92-46ae-bfbc-54e2d58006bb"),
                Email = "newemail@mail.com",
                FirstName = "New",
                LastName = "Name",
                Password = "newpassword",
                Role = "admin"
            };
            var adminJson = JsonConvert.SerializeObject(newAdmin);
            var data = new StringContent(adminJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Admin", data);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAdmin_BadContentsTest()
        {
            // Arrange
            var newData = new { Value1 = "String", Value2 = 10 };
            var dataJson = JsonConvert.SerializeObject(newData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Admin", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAdmin_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Admin/{DbSeed.Admin.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAdmin_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Admin/{DbSeed.Volunteer.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
