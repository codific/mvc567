using System;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Dtos.Entities
{
    public class SimpleFileDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string RelativeUrl { get; set; }

        public FileTypes FileType { get; set; }

        public FileExtensions FileExtension { get; set; }

        public ApplicationRoots Root { get; set; }

        public bool Temp { get; set; }
    }
}
