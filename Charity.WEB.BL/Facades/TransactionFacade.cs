//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.Common.Models;

namespace Charity.WEB.BL.Facades
{
    public class TransactionFacade : FacadeBase<TransactionDetailModel, TransactionListModel>
    {
        private readonly ITransactionApiClient _apiClient;
        public TransactionFacade(ITransactionApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public override async Task<List<TransactionListModel>> GetAllAsync()
        {
            var TransactionList = new List<TransactionListModel>();

            var Transaction = await _apiClient.TransactionGetAsync();
            TransactionList.AddRange(Transaction);

            return TransactionList;
        }
        public override async Task<TransactionDetailModel> GetByIdAsync(Guid id)
        {
            return await _apiClient.TransactionGetAsync(id);
        }
        public override async Task<Guid> CreateAsync(TransactionDetailModel data)
        {
            return await _apiClient.TransactionPostAsync(data);
        }
        public override async Task<Guid> UpdateAsync(TransactionDetailModel data)
        {
            return await _apiClient.TransactionPutAsync(data);
        }
        public override async Task DeleteAsync(Guid id)
        {
            await _apiClient.TransactionDeleteAsync(id);
        }

        public override async Task<List<TransactionListModel>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }
    }
}