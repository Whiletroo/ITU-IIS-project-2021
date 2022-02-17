//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class DonationFacade : FacadeBase<DonationDetailModel, DonationListModel>
    {
        private readonly IDonationApiClient _apiClient;
        public DonationFacade(IDonationApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<DonationListModel>> GetAllAsync()
        {
            var donationList = new List<DonationListModel>();
            var donation = await _apiClient.DonationGetAsync();
            donationList.AddRange(donation);
            return donationList;
        }
        public override async Task<DonationDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.DonationGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(DonationDetailModel data)
        {
            return await _apiClient.DonationPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(DonationDetailModel data)
        {
            return await _apiClient.DonationPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.DonationDeleteAsync(id);
        }
        public override async Task<List<DonationListModel>> SearchAsync(string search)
        {
            var donationList = new List<DonationListModel>();

            var donations = await _apiClient.SearchAsync(search);
            donationList.AddRange(donations);

            return donationList;
        }
    }
}