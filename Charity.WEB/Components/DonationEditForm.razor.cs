using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;


namespace Charity.WEB
{
    public partial class DonationEditForm 
    { 
        [Inject]
        private DonationFacade DonationFacade { get; set; } = null!;
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public ICollection<ShelterListModel> ShelterList { get; set; } = new List<ShelterListModel>();
       
        public DonationDetailModel Data { get; set; } = new DonationDetailModel();
        private DateTime? SelectedDateTime { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            ShelterList = await ShelterFacade.GetAllAsync();

            if (Id != Guid.Empty)
            {
                Data = await DonationFacade.GetByIdAsync(Id);
                SelectedDateTime = Data.DateTime;
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            Data.ShelterTitle = ShelterList.FirstOrDefault(s => s.Id.Equals(Data.ShelterId)).Title;
            Data.DateTime = SelectedDateTime;
            await DonationFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();

        }

        public async Task Create()
        {
            Data.ShelterTitle = ShelterList.FirstOrDefault(s => s.Id.Equals(Data.ShelterId)).Title;
            Data.DateTime = SelectedDateTime;
            await DonationFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await DonationFacade.DeleteAsync(Id);
            await NotifyOnModification();
        }

        private async Task NotifyOnModification()
        {
            if (OnModification.HasDelegate)
            {
                await OnModification.InvokeAsync(null);
            }
        }

        public DateOnly SelectedDate
        {
            get
            {
                if (SelectedDateTime != null)
                {
                    return DateOnly.Parse(SelectedDateTime.Value.ToString("dd/MM/yyyy"));
                }

                return DateOnly.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            }
            set
            {
                SelectedDateTime = DateTime.Parse(value.ToString("dd/MM/yyyy") + " " + SelectedTime);
            }
        }

        public TimeOnly SelectedTime
        {
            get
            {
                if (SelectedDateTime != null)
                {
                    return TimeOnly.Parse(SelectedDateTime.Value.ToString("HH:mm"));
                }

                return TimeOnly.Parse(DateTime.Now.ToString("HH:mm"));
            }
            set
            {
                SelectedDateTime = DateTime.Parse(SelectedDate + " " + value.ToString("HH:mm"));
            }
        }
    }
}
