//!--Author xkrukh00-- >
using System.Text;
using Blazored.LocalStorage;
using Charity.Common.Models;
using Charity.WEB.BL.Features;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Charity.WEB.BL.Facades
{
    public class AccountFacade : IAppFacade
    {
        private readonly IAccountApiClient _apiClient;
        private readonly CharityAuthStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AccountFacade(IAccountApiClient apiClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _apiClient = apiClient;
            _authStateProvider = (CharityAuthStateProvider) authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginModel userModel)
        {
            var result = await _apiClient.LoginAsync(userModel);

            if (!result.IsAuthSuccessful || result == null)
            {
                return result;
            }

            _localStorage.SetItemAsync("authToken", result.Token);
            _authStateProvider.NotifyUserAuthentication(result.Token);
            _apiClient.SetDefaultAuthorizationToken(result.Token);

            return result;
        }

        public async Task<RegistrationResponseModel> RegisterAsync(RegistrationModel userForRegistration)
        {
            return await _apiClient.RegisterAsync(userForRegistration);
        }

        public async Task<RegistrationResponseModel> RegisterShelterAdminAsync(RegistrationModel userForRegistration)
        {
            return await _apiClient.RegisterShelterAdminAsync(userForRegistration);
        }

        public async Task<RegistrationResponseModel> RegisterAdministratorAsync(RegistrationModel userForRegistration)
        {
            return await _apiClient.RegisterShelterAdminAsync(userForRegistration);
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _authStateProvider.NotifyUserLogout();
            _apiClient.SetDefaultAuthorizationNull();
        }
    }
}