using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages
{
    public partial class ShelterListPage
    {
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;

        private ICollection<ShelterListModel> ShelterList { get; set; } = new List<ShelterListModel>();
        private string SearchString { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                await LoadData();
            }
            else
            {
                await SearchData();
            }

            StateHasChanged();
        }

        private async Task LoadData()
        {
            ShelterList = await ShelterFacade.GetAllAsync();
        }

        private async Task SearchData()
        {
            ShelterList = await ShelterFacade.SearchAsync(SearchString);
        }
    }
}