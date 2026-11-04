namespace Application.UseCases.onexuser.Create;

public sealed class CreateOnexClientUserResponse
{
    public bool Succeeded { get; init; }

    public bool Created { get; init; }

    public string? UserId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string CardCode { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string[] Errors { get; init; } = Array.Empty<string>();
}