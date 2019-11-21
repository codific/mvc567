using System;
using System.Collections.Generic;

namespace Codific.Mvc567.Dtos.ViewModels
{
    public class SidebarMenuSectionItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ItemController { get; set; }

        public string ItemAction { get; set; }

        public string ItemArea { get; set; }

        public int Order { get; set; }

        public bool Single { get; set; }

        public string Icon { get; set; }

        public Guid AdminNavigationSchemeId { get; set; }

        public ICollection<SidebarNavigationLinkItemViewModel> Children { get; set; }
    }
}
