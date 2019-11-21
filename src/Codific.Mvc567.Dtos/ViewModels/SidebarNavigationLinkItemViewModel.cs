using System;

namespace Codific.Mvc567.Dtos.ViewModels
{
    public class SidebarNavigationLinkItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ItemController { get; set; }

        public string ItemAction { get; set; }

        public string ItemArea { get; set; }

        public int Order { get; set; }

        public Guid ParentSectionId { get; set; }
    }
}
