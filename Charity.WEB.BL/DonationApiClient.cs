//!--Author xkrukh00-- >
using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class DonationApiClient
    {
        public DonationApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}

