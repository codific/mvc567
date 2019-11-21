using System;
using AutoMapper;
using Codific.Mvc567.Dtos.Entities;
using Codific.Mvc567.Dtos.ViewModels;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.Profiles
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            this.CreateMap<File, SimpleFileDto>().ReverseMap();

            this.CreateMap<Language, SimpleLanguageDto>().ReverseMap();

            this.CreateMap<User, SimpleUserDto>().ReverseMap();

            this.CreateMap<AdminNavigationScheme, AdminNavigationSchemeViewModel>().ReverseMap();

            this.CreateMap<SidebarMenuSectionItem, SidebarMenuSectionItemViewModel>().ReverseMap();

            this.CreateMap<SidebarNavigationLinkItem, SidebarNavigationLinkItemViewModel>().ReverseMap();
        }
    }
}
