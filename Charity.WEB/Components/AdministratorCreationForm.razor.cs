using Microsoft.AspNetCore.Components;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB
{
    public partial class AdministratorCreationForm
    {
        [Inject] private AccountFacade AccountFacade { get; set; } = null!;
        [Inject] private ShelterFacade ShelterFacade { get; set; } = null!;
        [Inject] private ShelterAdminFacade ShelterAdminFacade { get; set; } = null!;

        [Parameter]
        public EventCallback OnModification { get; set; }

        private ShelterAdminDetailModel ShelterAdmin { get; set; }
        private AdminDetailModel Admin { get; set; }
        private List<ShelterListModel> ShelterList { get; set; } = new List<ShelterListModel>();
        private Guid SelectedShelterId { get; set; }
        private bool CreateShelterAdminButtonDisabled { get; set;}
        private List<string> Errors {get;set;}
        private bool ShowErrors { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ShelterAdmin = new ShelterAdminDetailModel() { Role = "ShelterAdministrator" };
            Admin = new AdminDetailModel() { Role = "Administrator" };
            ShelterList = await GetFreeShelters();
            SelectedShelterId = new Guid();
            SelectedShelterId = Guid.Empty;
            CreateShelterAdminButtonDisabled = false;
            Errors = new List<string>() {};
            ShowErrors = false;
        }

        public async Task CreateShelterAdmin()
        {
            CreateShelterAdminButtonDisabled = true;
            var registrationModel = CreateRegModelFromShelterAdminModel();
            var result = await AccountFacade.RegisterShelterAdminAsync(registrationModel);

            if(!result.IsSuccessfulRegistration)
            {
                CreateShelterAdminButtonDisabled = false;
                Errors = new List<string>() {};
                foreach (var error in result.Errors)
                {
                    Errors.Add(error);
                }
                ShowErrors = true;
                StateHasChanged();
                return;
            }

            var ShelterAdminDetail = await GetShelterAdminByEmail(ShelterAdmin.Email);
            ShelterAdminDetail.ShelterId = SelectedShelterId;
            await ShelterAdminFacade.UpdateAsync(ShelterAdminDetail);

            await NotifyOnModification();
        }

        private async Task<List<ShelterListModel>> GetFreeShelters()
        {
            var allSheltersList = await ShelterFacade.GetAllAsync();
            var result = new List<ShelterListModel>() {};
            foreach (var shelter in allSheltersList)
            {
                var detailedShelter = await ShelterFacade.GetByIdAsync(shelter.Id);
                if (detailedShelter.ShelterAdminId == null || detailedShelter.ShelterAdminId.Equals(Guid.Empty)) 
                    result.Add(shelter);
            }
            return result;
        }

        private RegistrationModel CreateRegModelFromShelterAdminModel()
        {
            return new RegistrationModel() {
                FirstName = ShelterAdmin.FirstName,
                LastName = ShelterAdmin.LastName,
                Email = ShelterAdmin.Email,
                PhotoURL = ShelterAdmin.PhotoURL,
                Phone = ShelterAdmin.Phone,
                Password = ShelterAdmin.Password,
                ConfirmPassword = ShelterAdmin.Password
            };
        }

        private async Task<ShelterAdminDetailModel> GetShelterAdminByEmail(string email){
            var shelterAdminsList = await ShelterAdminFacade.GetAllAsync();
            var shelterAdminListModel = shelterAdminsList.FirstOrDefault(sa => sa.Email == email);
            
            if (shelterAdminListModel != null) 
            {
                return await ShelterAdminFacade.GetByIdAsync(shelterAdminListModel.Id);
            } 
            else 
            {
                return null;
            }
        }

        private async Task NotifyOnModification()
        {
            if (OnModification.HasDelegate)
            {
                await OnModification.InvokeAsync(null);
            }
        }
    }
}
