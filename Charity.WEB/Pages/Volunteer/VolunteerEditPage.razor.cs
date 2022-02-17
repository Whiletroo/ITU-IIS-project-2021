using Microsoft.AspNetCore.Components;
using System;

namespace Charity.WEB.Pages
{
    public partial class VolunteerEditPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;

        [Parameter]
        public Guid Id { get; set; }

        public void NavigateBack()
        {
            navigationManager.NavigateTo($"/volunteers");
        }
    }
}