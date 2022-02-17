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
    public class TransactionApiTests : IAsyncDisposable
    {
        public readonly CharityApiApplicationFactory _application;
        public readonly Lazy<HttpClient> _client;
        public readonly Mapper _mapper;

        public TransactionApiTests()
        {
            _application = new CharityApiApplicationFactory();
            _client = new Lazy<HttpClient>(_application.CreateClient());

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetAllTransaction_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync("api/Transaction");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var transactions = await response.Content.ReadFromJsonAsync<ICollection<TransactionListModel>>();
            Assert.NotNull(transactions);
            Assert.NotEmpty(transactions);
        }


        [Fact]
        public async Task GetTransactionById_Test()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Transaction/{DbSeed.Transaction.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var transaction = await response.Content.ReadFromJsonAsync<TransactionListModel>();
            Assert.NotNull(transaction);
        }

        [Fact]
        public async Task GetTransactionById_NotFoundTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Transaction/df2e39d9-f691-4f9b-8533-61c2544c23f7");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetTransactionById_BadIdTest()
        {
            //Arrange

            //Act
            var response = await _client.Value.GetAsync($"api/Transaction/asdf1234");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateTransaction_SuccessTest()
        {
            // Arrange
            var newTransaction = new TransactionDetailModel()
            {
                DateTime = new DateTime(2021, 12, 21, 15, 21, 10),
                Sum = 200,
                VolunteerEmail = "volunteer@email.com",
                DonationId = Guid.Parse("0e3cb2af-6f89-4188-9415-b7bc64433d41"),
                VolunteerId = Guid.Parse("5a824d30-d369-48df-ac03-bb36c8c023e3")

            };
            var TransactionTeamJson = JsonConvert.SerializeObject(newTransaction);
            var TransactionData = new StringContent(TransactionTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Transaction", TransactionData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, createdId);
        }

        [Fact]
        public async Task CreateTransaction_BadContentTest()
        {
            // Arrange
            var TransactionnewData = new { Value1 = "String", Value2 = 10 };
            var TransactionDataJson = JsonConvert.SerializeObject(TransactionnewData);
            var TransactionData = new StringContent(TransactionDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PostAsync("/api/Transaction", TransactionData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTransaction_SuccessTest()
        {
            // Arrange
            var oldTransaction = _mapper.Map<TransactionDetailModel>(DbSeed.Transaction);
            var newTransaction = new TransactionDetailModel()
            {
                Id = oldTransaction.Id,
                DateTime = new DateTime(2021, 12, 21, 15, 21, 10),
                Sum = 200,
                VolunteerEmail = oldTransaction.VolunteerEmail,
                DonationId = oldTransaction.DonationId,
                DonationTitle = oldTransaction.DonationTitle,
                VolunteerId = oldTransaction.VolunteerId

            };
            var TransactionTeamJson = JsonConvert.SerializeObject(newTransaction);
            var TransactionData = new StringContent(TransactionTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Transaction", TransactionData);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var updatedId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.Equal(newTransaction.Id, updatedId);
        }


        [Fact]
        public async Task UpdateTrnsactin_BadIdTest()
        {
            // Arrange
            var newTransaction = new TransactionDetailModel()
            {
                Id = Guid.Parse("8dcfdc18-1f92-46ae-bfbc-80e2d58006aa"),
                DateTime = new DateTime(2021, 12, 21, 15, 21, 10),
                Sum = 200,
                VolunteerEmail = "Old_volunteer@email.com",
                DonationId = Guid.Parse("5af6bbb5-a2ae-433a-b999-d7b3891eb51b"),
                VolunteerId = Guid.Parse("952f40f0-8181-4cc6-aff8-d932e002d98f")
            };
            var TransactionTeamJson = JsonConvert.SerializeObject(newTransaction);
            var TransactionData = new StringContent(TransactionTeamJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Transaction", TransactionData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateTransaction_BadContentsTest()
        {
            // Arrange
            var newtrnsactionData = new { Value1 = "String", Value2 = 10 };
            var trnsactionDataJson = JsonConvert.SerializeObject(newtrnsactionData);
            var TrnsactionData = new StringContent(trnsactionDataJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.Value.PutAsync("/api/Transaction", TrnsactionData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTransaction_SuccessTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Transaction/{DbSeed.Transaction.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTransaction_BadIdTest()
        {
            // Arrange

            // Act
            var response = await _client.Value.DeleteAsync($"/api/Transaction/{DbSeed.Shelter.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public async ValueTask DisposeAsync()
        {
            await _application.DisposeAsync();
        }
    }
}
