//!--Author xkrukh00-- >
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Charity.WEB.BL.Facades;

namespace Charity.WEB.BL.Installers
{
    public class WebBLInstaller
    {
        public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
        {
            serviceCollection.AddTransient<IShelterApiClient, ShelterApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new ShelterApiClient(client, apiBaseUrl);
            });
            serviceCollection.AddTransient<IVolunteerApiClient, VolunteerApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new VolunteerApiClient(client, apiBaseUrl);
            });
            serviceCollection.AddTransient<IVolunteeringApiClient, VolunteeringApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new VolunteeringApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IDonationApiClient, DonationApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new DonationApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<ITransactionApiClient, TransactionApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new TransactionApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IEnrollmentApiClient, EnrollmentApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new EnrollmentApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IAdminApiClient, AdminApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new AdminApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IShelterAdminApiClient, ShelterAdminApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new ShelterAdminApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<IAccountApiClient, AccountApiClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new AccountApiClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<ShelterFacade>();
            serviceCollection.AddTransient<VolunteerFacade>();
            serviceCollection.AddTransient<VolunteeringFacade>();
            serviceCollection.AddTransient<DonationFacade>();
            serviceCollection.AddTransient<TransactionFacade>();
            serviceCollection.AddTransient<EnrollmentFacade>();
            serviceCollection.AddTransient<AdminFacade>();
            serviceCollection.AddTransient<ShelterAdminFacade>();
            serviceCollection.AddTransient<AccountFacade>();
        }

        public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            client.BaseAddress = new Uri(apiBaseUrl);
            return client;
        }
    }
}
