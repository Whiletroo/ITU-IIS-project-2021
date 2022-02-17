using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class ShelterApiClient
    {
        public ShelterApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}