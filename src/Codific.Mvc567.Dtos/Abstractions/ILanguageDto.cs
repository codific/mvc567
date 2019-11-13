using System;
namespace Codific.Mvc567.Dtos.Abstractions
{
    public interface ILanguageDto
    {
        string Id { get; set; }

        string Code { get; set; }

        string Name { get; set; }

        string NativeName { get; set; }

        string ImageId { get; set; }

        string TranslationFileUrl { get; set; }

        DateTime? LastTranslationFileGeneration { get; set; }

        bool IsDefault { get; set; }
    }
}
