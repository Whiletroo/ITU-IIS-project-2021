// Author: xkrukh00
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class AdminFacade : FacadeBase<AdminDetailModel, AdminListModel>
    {
        private readonly IAdminApiClient _apiClient;
        public AdminFacade(IAdminApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<AdminListModel>> GetAllAsync()
        {
            var AdminList = new List<AdminListModel>();
            var Admin = await _apiClient.AdminGetAsync();
            AdminList.AddRange(Admin);
            return AdminList;
        }
        public override async Task<AdminDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.AdminGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(AdminDetailModel data)
        {
            return await _apiClient.AdminPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(AdminDetailModel data)
        {
            return await _apiClient.AdminPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.AdminDeleteAsync(id);
        }
        public override async Task<List<AdminListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}