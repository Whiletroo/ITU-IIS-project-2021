//< !--Author xkrukh00-- >
using Charity.Common.Models;
using Microsoft.AspNetCore.Components;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.Pages
{
    public partial class Index
    {
        [Inject] 
        public DonationFacade DonationFacade { get; set; } = null!;

        [Inject]
        public VolunteeringFacade VolunteeringFacade { get; set; } = null!;

        private ICollection<DonationListModel> DonationList { get; set; } = new List<DonationListModel>();
        private ICollection<DonationListModel> tmpDonationList { get; set; } = new List<DonationListModel>();
        private ICollection<VolunteeringListModel> VolunteeringList { get; set; } = new List<VolunteeringListModel>();
        private ICollection<VolunteeringListModel> tmpVolunteeringList { get; set; } = new List<VolunteeringListModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            tmpDonationList = await DonationFacade.GetAllAsync();
            if (tmpDonationList.Count > 4)
            {
                DonationList = tmpDonationList.OrderBy(d => d.DateTime).Reverse().Take(4).ToList();
            }
            else
            {
                DonationList = tmpDonationList;
            }
            
            tmpVolunteeringList = await VolunteeringFacade.GetAllAsync();
            if (tmpVolunteeringList.Count > 4)
            {
                VolunteeringList = tmpVolunteeringList.OrderBy(v => v.DateTime).Reverse().Take(4).ToList();
            }
            else
            {
                VolunteeringList = tmpVolunteeringList;
            }
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
    }
}