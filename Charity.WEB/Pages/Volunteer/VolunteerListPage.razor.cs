using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages
{
    public partial class VolunteerListPage
    {
        [Inject]
        private VolunteerFacade VolunteerFacade { get; set; } = null!;

        private ICollection<VolunteerListModel> VolunteerList { get; set; } = new List<VolunteerListModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            VolunteerList = await VolunteerFacade.GetAllAsync();
        }
    }
}