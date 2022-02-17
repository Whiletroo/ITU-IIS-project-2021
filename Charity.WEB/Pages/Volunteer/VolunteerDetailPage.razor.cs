using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;
using Microsoft.AspNetCore.Components.Authorization;

namespace Charity.WEB.Pages
{
    public partial class VolunteerDetailPage
    {
        [Inject]
        private VolunteerFacade VolunteerFacade { get; set; } = null!;
        [Inject] 
        private AuthenticationStateProvider stateProvider { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        private VolunteerDetailModel Volunteer { get; set; } = null!;
        private AuthenticationState authState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            authState = await stateProvider.GetAuthenticationStateAsync();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            Volunteer = await VolunteerFacade.GetByIdAsync(Id);
        }
    }
}