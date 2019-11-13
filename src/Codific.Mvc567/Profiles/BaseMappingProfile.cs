using System;
using AutoMapper;
using Codific.Mvc567.Dtos.Entities;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.Profiles
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            CreateMap<File, SimpleFileDto>().ReverseMap();

            CreateMap<Language, SimpleLanguageDto>().ReverseMap();
        }
    }
}
