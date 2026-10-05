namespace BookStore.GUI.Models;

public sealed record Genre(
    Guid Id,
    string Name);

public sealed record CreateGenreRequest(
    string Name);

public sealed record UpdateGenreRequest(
    string Name);
