/*
 *  File:   VolunteeringFacade.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class VolunteeringFacade : FacadeBase<VolunteeringDetailModel, VolunteeringListModel>
    {
        private readonly IVolunteeringApiClient _apiClient;
        public VolunteeringFacade(IVolunteeringApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<VolunteeringListModel>> GetAllAsync()
        {
            var VolunteeringList = new List<VolunteeringListModel>();

            var Volunteering = await _apiClient.VolunteeringGetAsync();
            VolunteeringList.AddRange(Volunteering);

            return VolunteeringList;
        }
        public override async Task<VolunteeringDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.VolunteeringGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(VolunteeringDetailModel data)
        {
            return await _apiClient.VolunteeringPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(VolunteeringDetailModel data)
        {
            return await _apiClient.VolunteeringPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.VolunteeringDeleteAsync(id);
        }

        public override async Task<List<VolunteeringListModel>> SearchAsync(string search)
        {
            var volunteeringList = new List<VolunteeringListModel>();

            var volunteerings = await _apiClient.SearchAsync(search);
            volunteeringList.AddRange(volunteerings);

            return volunteeringList;
        }
    }
}
