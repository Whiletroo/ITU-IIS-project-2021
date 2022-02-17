//!--Author xkrukh00-- >
using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class VolunteerApiClient
    {
        public VolunteerApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}