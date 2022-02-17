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
    public class DonationApiTests : IAsyncDisposable
    {

        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public DonationApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }


        [Fact]
        public async Task GetAllDonation_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Donation");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donations = await response.Content.ReadFromJsonAsync<ICollection<DonationListModel>>();
            Assert.NotNull(donations);
            Assert.NotEmpty(donations);
        }

        [Fact]
        public async Task GetDonationById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Donation/{DbSeed.Donation.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donation = await response.Content.ReadFromJsonAsync<DonationListModel>();
            Assert.NotNull(donation);
        }

        [Fact]
        public async Task GetDonationById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Donation/df2e39d9-f691-4f9b-8533-61c2544c23f7");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDonationById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Donation/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateDonation_SuccessTest()
        {
            // Arrange
            var newDonation = new DonationDetailModel()
            {
                Title = "Donation title",
                Goal = 50000,
                ShelterTitle = "Shelter Title",
                ShelterId = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5"),
            };
            var DonationTeamJson = JsonConvert.SerializeObject(newDonation);
            var DonationData = new StringContent(DonationTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Donation", DonationData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateDonation_BadContentTest()
        {
            // Arrange
            var newDonationData = new { Value1 = "String", Value2 = 10 };
            var DonationDataJson = JsonConvert.SerializeObject(newDonationData);
            var DonationData = new StringContent(DonationDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Donation", DonationData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateDonation_SuccessTest()
        {
            // Arrange
            var oldDonation = _mapper.Map<DonationDetailModel>(DbSeed.Donation);
            var newDonation = new DonationDetailModel()
            {
                Id = oldDonation.Id,
                Title = "Donation Title",
                Goal = 50000,
                ShelterTitle = "Shelter Title",
                ShelterId = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5"),

            };
            var teamJson = JsonConvert.SerializeObject(newDonation);
            var data = new StringContent(teamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Donation", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newDonation.Id, updatedId);
        }

        [Fact]
        public async Task UpdateDonation_BadIdTest()
        {
            // Arrange
            var newDonation = new DonationDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                Title = "Donation Title",
                Goal = 50000,
                ShelterTitle = "Shelter Title",
                ShelterId = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5"),
            };
            var DonationTeamJson = JsonConvert.SerializeObject(newDonation);
            var DonationData = new StringContent(DonationTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Donation", DonationData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateDonation_BadContentsTest()
        {
            // Arrange
            var newDonationData = new { Value1 = "String", Value2 = 10 };
            var DonationDataJson = JsonConvert.SerializeObject(newDonationData);
            var DonationData = new StringContent(DonationDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Donation", DonationData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDonation_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Donation/{DbSeed.Donation.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDonation_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Donation/{DbSeed.Donation.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchDonation_SuccessTest1()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Donation/Search?search=donation");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donations = await response.Content.ReadFromJsonAsync<ICollection<DonationListModel>>();
            Assert.NotNull(donations);
            Assert.NotEmpty(donations);
        }

        [Fact]
        public async Task SearchDonation_SuccessTest2()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Donation/Search?search=Doggie");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donations = await response.Content.ReadFromJsonAsync<ICollection<DonationListModel>>();
            Assert.NotNull(donations);
            Assert.NotEmpty(donations);
        }

        [Fact]
        public async Task SearchDonation_SuccessTest3()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Donation/Search?search=cute dog");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donations = await response.Content.ReadFromJsonAsync<ICollection<DonationListModel>>();
            Assert.NotNull(donations);
            Assert.NotEmpty(donations);
        }

        [Fact]
        public async Task SearchDonation_BadRequestTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Donation/Search");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchDonation_NotFoundTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.GetAsync($"/api/Donation/Search?search=No donations");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var donations = await response.Content.ReadFromJsonAsync<ICollection<DonationListModel>>();
            Assert.NotNull(donations);
            Assert.Empty(donations);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
