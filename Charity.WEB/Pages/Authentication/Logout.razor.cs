//< !--Author xkrukh00-- >*@
using Microsoft.AspNetCore.Components;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        public AccountFacade AccountFacade { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AccountFacade.LogoutAsync();
            NavigationManager.NavigateTo("/", true);
        }
    }
}
