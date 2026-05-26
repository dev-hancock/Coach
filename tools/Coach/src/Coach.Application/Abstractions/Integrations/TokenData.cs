namespace Coach.Application.Abstractions.Integrations;

public sealed record TokenData(
    long ExternalId,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);