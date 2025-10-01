using System;
using System.ComponentModel.DataAnnotations;

namespace LegacyBookStore.DTO;

public record UserResponse(
    int Id,
    string Name,
    [EmailAddress]
    string Email,
    string Role
);
