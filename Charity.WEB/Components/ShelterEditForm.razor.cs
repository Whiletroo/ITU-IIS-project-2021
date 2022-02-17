using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;


namespace Charity.WEB
{
    public partial class ShelterEditForm
    {
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        public ICollection<DonationListModel> Donations { get; set; } = new List<DonationListModel> { };
        public ICollection<VolunteeringListModel> Volunteerings { get; set; } = new List<VolunteeringListModel> { };

        [Parameter]
        public EventCallback OnModification { get; set; }

        public ShelterDetailModel Data { get; set; } = new ShelterDetailModel();

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Data = await ShelterFacade.GetByIdAsync(Id);
                Donations = Data.Donations;
                Volunteerings = Data.Volunteerings;
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            await ShelterFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            await ShelterFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await ShelterFacade.DeleteAsync(Id);
            await NotifyOnModification();
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
