//!--Author xkrukh00-- >
using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class ShelterAdminApiClient
    {
        public ShelterAdminApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}