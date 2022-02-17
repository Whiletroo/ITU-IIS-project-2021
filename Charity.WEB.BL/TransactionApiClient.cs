using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class TransactionApiClient
    {
        public TransactionApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
