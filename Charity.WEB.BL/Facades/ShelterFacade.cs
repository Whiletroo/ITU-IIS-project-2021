using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class ShelterFacade : FacadeBase<ShelterDetailModel, ShelterListModel>
    {
        private readonly IShelterApiClient _apiClient;

        public ShelterFacade(IShelterApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override async Task<List<ShelterListModel>> GetAllAsync()
        {
            var shelterList = new List<ShelterListModel>();

            var shelter = await _apiClient.ShelterGetAsync();
            shelterList.AddRange(shelter);

            return shelterList;
        }

        public override async Task<ShelterDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.ShelterGetAsync(id);
        }

        public override async Task<Guid> CreateAsync(ShelterDetailModel data)
        {
            return await _apiClient.ShelterPostAsync(data);
        }

        public override async Task<Guid> UpdateAsync(ShelterDetailModel data)
        {
            return await _apiClient.ShelterPutAsync(data);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.ShelterDeleteAsync(id);
        }

        public override async Task<List<ShelterListModel>> SearchAsync(string search)
        {
            var shelterList = new List<ShelterListModel>();

            var shelters = await _apiClient.SearchAsync(search);
            shelterList.AddRange(shelters);

            return shelterList;
        }
    }
}