//< !--Author xkrukh00-- >*@
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Charity.Common.Models;
using Charity.WEB.BL;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages.Authentication
{
    public partial class RegistrationPage
    {
        [Inject]
        public AccountFacade AccountFacade { get;set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private RegistrationModel _userForRegistration = new RegistrationModel();
        public bool ShowRegistrationErrors { get; set; }
        public bool ShowRegistrationSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool RegisterButtonDisabled { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RegisterButtonDisabled = false;
            await base.OnInitializedAsync();
        }

        public async Task ExecuteRegistration()
        {
            ShowRegistrationErrors = false;
            var result = await AccountFacade.RegisterAsync(_userForRegistration);
            if (!result.IsSuccessfulRegistration)
            {
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
            else
            {
                ShowRegistrationSuccess = true;
                RegisterButtonDisabled = true;
                StateHasChanged();
                await Task.Delay(1000);
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
