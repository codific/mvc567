using System;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Dtos.Abstractions
{
    public interface IFileDto
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Path { get; set; }

        string RelativeUrl { get; set; }

        FileTypes FileType { get; set; }

        FileExtensions FileExtension { get; set; }
    }
}
