using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Service.Dtos
{
    public record UserDto(Guid Id, string UserName, string Email, decimal Amount, DateTime CreatedDate);
    public record UpdateUserDto([Required][EmailAddress] string Email, [Range(0, 100000)] decimal Amount);
}