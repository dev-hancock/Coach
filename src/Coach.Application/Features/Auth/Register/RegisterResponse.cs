namespace Coach.Application.Features.Auth.Register;

public sealed record RegisterResponse(Guid UserId, Guid AthleteId, string Email);
