using System.Collections.Generic;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;

namespace Codific.Mvc567.Dtos.ViewModels
{
    public class AdminNavigationSchemeViewModel : CreateEditEntityViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<SidebarMenuSectionItemViewModel> Menus { get; set; }
    }
}
