/*
 *  File:   VolunteeringApiClient.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Net.Http;

namespace Charity.WEB.BL
{
    public partial class VolunteeringApiClient
    {
        public VolunteeringApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
