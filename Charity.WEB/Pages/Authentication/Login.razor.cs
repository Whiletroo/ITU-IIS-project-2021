//< !--Author xkrukh00-- >*@
using Microsoft.AspNetCore.Components;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        public AccountFacade AccountFacade { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        private LoginModel _loginModel = new LoginModel();
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await AccountFacade.LoginAsync(_loginModel);
            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/", true);
            }
        }
    }
}
