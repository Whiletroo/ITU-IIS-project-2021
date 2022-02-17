//< !--Author xkrukh00-- > *@*@
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Charity.Common.Models;
using Charity.WEB.BL.Facades;


namespace Charity.WEB
{
    public partial class VolunteerEditForm
    {
        [Inject]
        private VolunteerFacade VolunteerFacade { get; set; } = null!;

        [Parameter]
        public Guid Id { get; init; }

        [Parameter]
        public EventCallback OnModification { get; set; }

        public VolunteerDetailModel Data { get; set; } = new VolunteerDetailModel();

        protected override async Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Data = await VolunteerFacade.GetByIdAsync(Id);
            }

            await base.OnInitializedAsync();
        }

        public async Task Update()
        {
            await VolunteerFacade.UpdateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Create()
        {
            await VolunteerFacade.CreateAsync(Data);
            Data = new();
            await NotifyOnModification();
        }

        public async Task Delete()
        {
            await VolunteerFacade.DeleteAsync(Id);
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