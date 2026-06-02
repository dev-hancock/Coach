namespace Coach.Application.Features.Auth.Login;

public sealed record LoginResponse(
    Guid UserId,
    Guid AthleteId,
    string Email);
