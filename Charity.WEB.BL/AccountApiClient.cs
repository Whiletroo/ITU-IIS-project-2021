//!--Author xkrukh00-- >
using System.Net.Http;
using System.Net.Http.Headers;

namespace Charity.WEB.BL
{
    public partial class AccountApiClient
    {
        public AccountApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }

        public void SetDefaultAuthorizationNull()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void SetDefaultAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
    }
}