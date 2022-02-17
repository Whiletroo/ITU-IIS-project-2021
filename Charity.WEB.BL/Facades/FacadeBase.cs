//!--Author xkrukh00-- >
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.BL.Facades
{
    public abstract class FacadeBase<TDetailModel, TListModel> : IAppFacade
    {
        protected virtual string apiVersion => "3";
        protected virtual string culture => "en";

        public abstract Task<List<TListModel>> GetAllAsync();
        public abstract Task<TDetailModel> GetByIdAsync(Guid id);
        public abstract Task<Guid> CreateAsync(TDetailModel data);
        public abstract Task<Guid> UpdateAsync(TDetailModel data);
        public abstract Task DeleteAsync(Guid id);
        public abstract Task<List<TListModel>> SearchAsync(string search);
    }
}