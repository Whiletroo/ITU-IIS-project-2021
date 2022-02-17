// Author: xkrukh00
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class ShelterAdminFacade : FacadeBase<ShelterAdminDetailModel, ShelterAdminListModel>
    {
        private readonly IShelterAdminApiClient _apiClient;
        public ShelterAdminFacade(IShelterAdminApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<ShelterAdminListModel>> GetAllAsync()
        {
            var ShelterAdminList = new List<ShelterAdminListModel>();

            var ShelterAdmin = await _apiClient.ShelterAdminGetAsync();
            ShelterAdminList.AddRange(ShelterAdmin);

            return ShelterAdminList;
        }
        public override async Task<ShelterAdminDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.ShelterAdminGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(ShelterAdminDetailModel data)
        {
            return await _apiClient.ShelterAdminPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(ShelterAdminDetailModel data)
        {
            return await _apiClient.ShelterAdminPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.ShelterAdminDeleteAsync(id);
        }

        public override async Task<List<ShelterAdminListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}