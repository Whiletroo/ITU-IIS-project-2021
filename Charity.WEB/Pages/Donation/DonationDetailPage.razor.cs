using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;
using Microsoft.AspNetCore.Components.Authorization;
using Majorsoft.Blazor.Components.Notifications;

namespace Charity.WEB.Pages
{
    public partial class DonationDetailPage
    {
        [Inject]
        private IToastService _toastService { get; set; } = null!;
        [Inject]
        private DonationFacade DonationFacade { get; set; } = null!;
        [Inject]
        private TransactionFacade TransactionFacade { get; set; } = null!;
        [Inject] 
        private VolunteerFacade VolunteerFacade { get; set; } = null!;
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider stateProvider { get; set; } = null!;
        [Inject]
        public NavigationManager navigationManager { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }
        private TransactionDetailModel Transaction { get; set; }
        private DonationDetailModel Donation { get; set; } = null!;
        private AuthenticationState authState { get; set; }
        private int Amount { get; set; }
        private bool showAuthError { get; set; }
        private string ShelterAdminEmail { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            showAuthError = false;
            Amount = 0;
            authState = await stateProvider.GetAuthenticationStateAsync();
            var shelter = await ShelterFacade.GetByIdAsync(Donation.ShelterId);
            ShelterAdminEmail = shelter.ShelterAdminEmail;
            if (authState.User.Identity.IsAuthenticated && authState.User.IsInRole("Volunteer"))
            {
                var Volunteer = await FindVolunteerByMail(authState.User.Identity.Name);
                if (Volunteer != null)
                {
                    Transaction = new TransactionDetailModel()
                    {
                        VolunteerId = Volunteer.Id,
                        VolunteerFirstName = Volunteer.FirstName,
                        VolunteerLastName = Volunteer.LastName,
                        VolunteerEmail = Volunteer.Email,
                        DonationId = Donation.Id,
                        DonationTitle = Donation.Title
                    };
                }
            }

            

            await base.OnInitializedAsync();
        }
        public int GetPercents(DonationDetailModel donation)
        {
            double State = 0.0;
            if (donation.State != null)
            {
                State = Convert.ToDouble(donation.State);
            }
            int result = (int)( ( State / donation.Goal) * 100);
            return result >= 100 ? 100 : result;
        }

        private async Task CreateTransaction()
        {
            if (!authState.User.Identity.IsAuthenticated || !authState.User.IsInRole("Volunteer"))
            {
                showAuthError = true;
                StateHasChanged();
                return;
            }
            if (Amount > 0)
            {
                Transaction.DateTime = DateTime.Now;
                Transaction.Sum = Amount;
                await TransactionFacade.CreateAsync(Transaction);
                var oldSum = Donation.State;
                Donation = await DonationFacade.GetByIdAsync(Donation.Id);
                var newSum = Donation.State;

                if (oldSum != newSum)
                {
                    MyShowToast("Successfully donated", NotificationTypes.Success);
                } else
                {
                    MyShowToast("Error. Something went wrong", NotificationTypes.Danger);
                }
            }
        }

        private async Task LoadData()
        {
            Donation = await DonationFacade.GetByIdAsync(Id);
        }

        private async Task<VolunteerListModel> FindVolunteerByMail(string mail)
        {
            var volunteers = await VolunteerFacade.GetAllAsync();
            return volunteers.FirstOrDefault(v => v.Email == mail);
        }

        private async Task MyShowToast(string message, NotificationTypes type) {
            _toastService.ShowToast(new ToastSettings()
            {
                Content = builder => builder.AddMarkupContent(0, $@"<strong>" + message + "</strong>"),
                NotificationStyle = NotificationStyles.Normal,
                Type = type,
                AutoCloseInSec = 5,
                ShadowEffect = 5,
                ShowCloseButton = false,
                ShowCloseCountdownProgress = true,
                ShowIcon= true
            });
        }
    }
}