//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class VolunteerFacade : FacadeBase<VolunteerDetailModel, VolunteerListModel>
    {
        private readonly IVolunteerApiClient _apiClient;

        public VolunteerFacade(IVolunteerApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<VolunteerListModel>> GetAllAsync()
        {
            var shelterList = new List<VolunteerListModel>();

            var shelter = await _apiClient.VolunteerGetAsync();
            shelterList.AddRange(shelter);

            return shelterList;
        }

        public override async Task<VolunteerDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.VolunteerGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(VolunteerDetailModel data)
        {
            return await _apiClient.VolunteerPostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(VolunteerDetailModel data)
        {
            return await _apiClient.VolunteerPutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.VolunteerDeleteAsync(id);
        }

        public override async Task<List<VolunteerListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}