using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages
{
    public partial class DonationListPage
    {
        [Inject]
        private DonationFacade DonationFacade { get; set; } = null!;
        private ICollection<DonationListModel> DonationList { get; set; } = new List<DonationListModel>();
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
            DonationList = await DonationFacade.GetAllAsync();
        }

        public int GetPercents(DonationListModel donation)
        {
            double State = 0.0;
            if (donation.State != null)
            {
                State = Convert.ToDouble(donation.State);
            }
            int result = (int)((State / donation.Goal) * 100);
            return result >= 100 ? 100 : result;
        }
        private async Task SearchData()
        {
            DonationList = await DonationFacade.SearchAsync(SearchString);
        }

    }
}