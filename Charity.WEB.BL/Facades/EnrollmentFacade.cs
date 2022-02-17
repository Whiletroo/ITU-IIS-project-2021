using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class EnrollmentFacade : FacadeBase<EnrollmentDetailModel, EnrollmentListModel>
    {
        private readonly IEnrollmentApiClient _apiClient;
        public EnrollmentFacade(IEnrollmentApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<EnrollmentListModel>> GetAllAsync()
        {
            var EnrollmentList = new List<EnrollmentListModel>();

            var Enrollment = await _apiClient.EnrollmentGetAsync();
            EnrollmentList.AddRange(Enrollment);

            return EnrollmentList;
        }
        public override async Task<EnrollmentDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.EnrollmentGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(EnrollmentDetailModel data)
        {
            return await _apiClient.EnrollmentPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(EnrollmentDetailModel data)
        {
            return await _apiClient.EnrollmentPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.EnrollmentDeleteAsync(id);
        }

        public override async Task<List<EnrollmentListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}
