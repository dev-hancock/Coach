namespace Coach.Application.Features.Auth.Register;

public sealed record RegisterResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);
