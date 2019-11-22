using System;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(Role), ReverseMap = true)]
    public class RoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
