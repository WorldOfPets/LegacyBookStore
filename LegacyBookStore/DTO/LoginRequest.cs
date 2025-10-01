using System;

namespace LegacyBookStore.DTO;

public record LoginRequest(
 string username,
 string password
);
