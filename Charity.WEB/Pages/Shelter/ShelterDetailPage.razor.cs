using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;
using Microsoft.AspNetCore.Components.Authorization;

namespace Charity.WEB.Pages
{
    public partial class ShelterDetailPage
    {
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider stateProvider { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        private ShelterDetailModel Shelter { get; set; } = null!;
        private AuthenticationState authState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            authState = await stateProvider.GetAuthenticationStateAsync();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            Shelter = await ShelterFacade.GetByIdAsync(Id);
        }
    }
}