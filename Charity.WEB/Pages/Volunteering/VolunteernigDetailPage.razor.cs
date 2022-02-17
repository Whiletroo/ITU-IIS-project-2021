using Microsoft.AspNetCore.Components;
using System;
using System.Reflection.Metadata;
using Blazored.Modal;
using Blazored.Modal.Services;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;
using Microsoft.AspNetCore.Components.Authorization;
using Majorsoft.Blazor.Components.Notifications;
using Microsoft.JSInterop.WebAssembly;

namespace Charity.WEB.Pages
{
    public partial class VolunteernigDetailPage
    {
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Inject]
        private IToastService _toastService { get; set; } = null!;
        [Inject]
        private VolunteeringFacade VolunteeringFacade { get; set; } = null!;
        [Inject]
        private VolunteerFacade VolunteerFacade { get; set; } = null!;
        [Inject]
        private EnrollmentFacade EnrollmentFacade { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider stateProvider { get; set; } = null!;
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }
        private VolunteeringDetailModel Volunteering { get; set; } = null!;
        private EnrollmentDetailModel Enrollment { get; set; } = null!;
        private AuthenticationState authState { get; set; }
        private string ShelterAdminEmail { get; set; }
        private bool showAuthError { get; set; }
        private string error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            showAuthError = false;
            authState = await stateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                if (authState.User.IsInRole("ShelterAdministrator"))
                {
                    if (Volunteering.ShelterId != null)
                    {
                        var shelter = await ShelterFacade.GetByIdAsync(Volunteering.ShelterId ?? Guid.Empty);
                        if (shelter != null)
                        {
                            ShelterAdminEmail = shelter.ShelterAdminEmail;
                        }
                        else
                        {
                            ShelterAdminEmail = "";
                        }
                    }
                }

                if (authState.User.IsInRole("Volunteer"))
                {
                    var Volunteer = await FindVolunteerByMail(authState.User.Identity.Name);
                    if (Volunteer != null)
                    {
                        Enrollment = new EnrollmentDetailModel()
                        {
                            VolunteerEmail = Volunteer.Email,
                            VolunteerId = Volunteer.Id,
                            VolunteeringId = Volunteering.Id,
                            VolunteeringTitle = Volunteering.Title
                        };
                    }
                }
            }
            await base.OnInitializedAsync();
        }
        
        private async Task ShowEditVolunteers(Guid Id)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(VolunteeringEditForm.Id), Id);
            parameters.Add(nameof(VolunteeringEditForm.Callback), EventCallback.Factory.Create(this, LoadData));

            Modal.Show<VolunteeringEditForm>("Edit Volunteering", parameters);
        }


        public int GetPercents(VolunteeringDetailModel volunteering)
        {
            double State = 0.0;
            int Goal = volunteering.RequiredCount ?? 0;
            if (volunteering.Enrollments != null)
            {
                State = Convert.ToDouble(volunteering.Enrollments.Count());
            }
            int result = (int)((State / Goal) * 100);
            return result >= 100 ? 100 : result;
        }

        private async Task LoadData()
        {
            Volunteering = await VolunteeringFacade.GetByIdAsync(Id);
        }

        private async Task Enroll()
        {
            if (!authState.User.Identity.IsAuthenticated || !authState.User.IsInRole("Volunteer"))
            {
                showAuthError = true;
                error = "You are not authorized or you are not volunteer.";
                StateHasChanged();
            }
            if (authState.User.Identity.IsAuthenticated && authState.User.IsInRole("Volunteer"))
            {
                if (!Volunteering.Enrollments.Any(e => e.VolunteerEmail == authState.User.Identity.Name))
                {
                    Enrollment.DateTime = DateTime.Now;
                    await EnrollmentFacade.CreateAsync(Enrollment);
                    var oldCount = Volunteering.Enrollments.Count;
                    Volunteering = await VolunteeringFacade.GetByIdAsync(Volunteering.Id);
                    var newCount = Volunteering.Enrollments.Count;
                    if (oldCount != newCount)
                    {
                        MyShowToast("Successfully enrolled", NotificationTypes.Success);
                    } else
                    {
                        MyShowToast("Error. Something went wrong", NotificationTypes.Danger);
                    }
                }
                else
                {
                    showAuthError = true;
                    error = "You are already enrolled";
                    StateHasChanged();
                }
            }
        }
        private async Task<VolunteerListModel> FindVolunteerByMail(string mail)
        {
            var volunteers = await VolunteerFacade.GetAllAsync();
            return volunteers.FirstOrDefault(v => v.Email == mail);
        }

        private async Task MyShowToast(string message, NotificationTypes type)
        {
            _toastService.ShowToast(new ToastSettings()
            {
                Content = builder => builder.AddMarkupContent(0, $@"<strong>" + message + "</strong>"),
                NotificationStyle = NotificationStyles.Normal,
                Type = type,
                AutoCloseInSec = 5,
                ShadowEffect = 5,
                ShowCloseButton = false,
                ShowCloseCountdownProgress = true,
                ShowIcon = true
            });
        }
    }
}
