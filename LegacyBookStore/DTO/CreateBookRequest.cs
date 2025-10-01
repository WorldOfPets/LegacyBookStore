using System;
using System.ComponentModel.DataAnnotations;

namespace LegacyBookStore.DTO;

public record BookRequest(
    string Title,
    string Author,
    [Range(0,(double)decimal.MaxValue)]
    decimal Price,
    string Description
);