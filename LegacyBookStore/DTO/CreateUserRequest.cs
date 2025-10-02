using System;
using System.ComponentModel.DataAnnotations;

namespace LegacyBookStore.DTO;

    public record CreateUserRequest(
        string Name,
        [EmailAddress]
        string Email,
        string Role
    );


