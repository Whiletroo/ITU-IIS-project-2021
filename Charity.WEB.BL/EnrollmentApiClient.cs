/*
 *  File:   EnrollmentApiClient.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.WEB.BL
{
    public partial class EnrollmentApiClient
    {
        public EnrollmentApiClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
