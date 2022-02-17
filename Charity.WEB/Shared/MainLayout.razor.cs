//<!-- Author xkrukh00-->

using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Charity.WEB.BL.Facades;
using Charity.WEB.BL.Features;
using Microsoft.AspNetCore.Components.Authorization;
using Majorsoft.Blazor.Components.Notifications;

namespace Charity.WEB.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private IToastService _toastService { get; set; } = null!;
        [Inject]
        public AccountFacade AccountFacade { get; set; }
        [Inject]
        public AuthenticationStateProvider StateProvider { get; set; }
        
        [Inject]
        public ShelterAdminFacade ShelterAdminFacade { get; set; }
        [Inject]
        public VolunteerFacade VolunteerFacade { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        

        private string? ProfileUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var AuthState = await StateProvider.GetAuthenticationStateAsync();
            if (AuthState.User.Identity.IsAuthenticated && ( AuthState.User.IsInRole("Volunteer") || AuthState.User.IsInRole("ShelterAdministrator") ) )
            {
                var User = AuthState.User;
                ProfileUrl = await GetProfileUrl(User);
            }

            //Common settings can be set globally for ToastContainer and Toasts in IToastService
            _toastService.GlobalSettings.Position = ToastPositions.TopCenter;
            _toastService.GlobalSettings.RemoveToastsOnNavigation = true;
            _toastService.GlobalSettings.Width = 420;
            _toastService.GlobalSettings.PaddingFromSide = -1;
            _toastService.GlobalSettings.PaddingFromTopOrBottom = 60;

            ToastContainerGlobalSettings.DefaultToastsAutoCloseInSec = 12;

            await base.OnInitializedAsync();
        }

        public async Task<string> GetProfileUrl(ClaimsPrincipal User)
        {
            var email = User.Identity.Name;
            string result = null;

            if (User.IsInRole("Volunteer"))
            {
                var volunteerList = await VolunteerFacade.GetAllAsync();
                var volunteer = volunteerList.FirstOrDefault(a => a.Email == email);
                if(volunteer != null)
                    return "/volunteers/" + volunteer.Id;
                return "";
            }
            else
            {
                var shelterAdminList = await ShelterAdminFacade.GetAllAsync();
                var shelterAdmin = shelterAdminList.FirstOrDefault(a => a.Email == email);
                if (shelterAdmin != null)
                    return "/shelters/" + shelterAdmin.ShelterId;
                return "/shelters";
            }
        }
        
        public void GoToUserProfile()
        {
            NavigationManager.NavigateTo(ProfileUrl);
        }

        public void GoToLoginPage()
        {
            NavigationManager.NavigateTo("/login");
        }

        public void GoToRegistrationPage()
        {
            NavigationManager.NavigateTo("/registration");
        }

        public void GoToCreateAdminPage()
        {
            NavigationManager.NavigateTo("/createadmin");
        }

        public void Logout()
        {
            NavigationManager.NavigateTo("/logout");
        }
    }
}