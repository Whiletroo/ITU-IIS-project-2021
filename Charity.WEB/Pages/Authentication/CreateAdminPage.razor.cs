using Microsoft.AspNetCore.Components;

namespace Charity.WEB.Pages
{
    public partial class CreateAdminPage
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = null!;

        public void NavigateBack()
        {
            navigationManager.NavigateTo($"/donations");
        }
    }
}