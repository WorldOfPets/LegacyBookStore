namespace LegacyBookStore.DTO;

public record GetBookResponse(
    int Id,
    string Title,
    string Author,
    decimal Price,
    string Description
);