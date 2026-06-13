namespace Coach.Application.Features.Auth.Login;

public sealed record LoginResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
