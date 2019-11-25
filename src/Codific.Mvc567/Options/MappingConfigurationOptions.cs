using System;
using System.Collections.Generic;
using AutoMapper;

namespace Codific.Mvc567.Options
{
    public class MappingConfigurationOptions
    {
        public MappingConfigurationOptions()
        {
            this.MappingAssemblies = new List<string>();
            this.MappingProfiles = new List<Type>();
        }

        public List<string> MappingAssemblies { get; private set; }

        public List<Type> MappingProfiles { get; private set; }

        public void AddProfile<TProfile>()
            where TProfile : Profile
        {
            this.MappingProfiles.Add(typeof(TProfile));
        }

        public void AddMaps(string assemblyString)
        {
            this.MappingAssemblies.Add(assemblyString);
        }
    }
}
