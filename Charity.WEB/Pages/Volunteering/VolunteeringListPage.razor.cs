using Microsoft.AspNetCore.Components;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages
{
    public partial class VolunteeringListPage
    {
        [Inject]
        private VolunteeringFacade VolunteeringFacade { get; set; } = null!;
        private ICollection<VolunteeringListModel> VolunteeringList { get; set; } = new List<VolunteeringListModel>();
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
            VolunteeringList = await VolunteeringFacade.GetAllAsync();
        }

        private async Task SearchData()
        {
            VolunteeringList = await VolunteeringFacade.SearchAsync(SearchString);
        }
    }
}
