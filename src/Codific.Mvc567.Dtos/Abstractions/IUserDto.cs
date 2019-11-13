using System;
namespace Codific.Mvc567.Dtos.Abstractions
{
    public interface IUserDto
    {
        string Id { get; set; }

        string Email { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Name { get; }

        DateTime RegistrationDate { get; set; }

        bool TwoFactorEnabled { get; set; }

        bool IsLockedOut { get; set; }
    }
}
