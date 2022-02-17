using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Blazored.Modal;
using Blazored.Modal.Services;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;

namespace Charity.WEB
{
    public partial class VolunteeringEditForm
    {
        [Parameter]
        public EventCallback Callback { get; set; }
        [CascadingParameter]
        public BlazoredModalInstance BlazoredModal { get; set; }
        [CascadingParameter]
        public IModalService Modal { get; set; }
        [Inject]
        private VolunteeringFacade VolunteeringFacade { get; set; } = null!;
        [Inject]
        private ShelterFacade ShelterFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public VolunteeringDetailModel Data { get; set; } = new VolunteeringDetailModel();

        public ICollection<ShelterListModel> ShelterList { get; set; } = new List<ShelterListModel>();
        private Guid? SelectedShelterId { get; set; } = Guid.Empty;
        private DateTime? SelectedDateTime { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            ShelterList = await ShelterFacade.GetAllAsync();
            if (Id != Guid.Empty)
            {
                Data = await VolunteeringFacade.GetByIdAsync(Id);
                SelectedShelterId = Data.ShelterId ?? Guid.Empty;
                SelectedDateTime = Data.DateTime;
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            Data.ShelterTitle = ShelterList.FirstOrDefault(s => s.Id.Equals(Data.ShelterId)).Title;
            Data.DateTime = SelectedDateTime;
            await VolunteeringFacade.UpdateAsync(Data);
            Data = new();
            await BlazoredModal.CloseAsync();
            await Callback.InvokeAsync();
            await NotifyOnModification();
        }

        public async Task Create()
        {
           
            Data.ShelterTitle = ShelterList.FirstOrDefault(s => s.Id.Equals(Data.ShelterId)).Title;
            Data.DateTime = SelectedDateTime;
            await VolunteeringFacade.CreateAsync(Data);
            Data = new();
            await BlazoredModal.CloseAsync();
            await Callback.InvokeAsync();
            await NotifyOnModification();
            
        }

        public async Task Delete()
        {
            await VolunteeringFacade.DeleteAsync(Id);
            await Callback.InvokeAsync();
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
