namespace Coach.Application.Abstractions.Identity;

/// <summary>
/// Represents the authenticated user context extracted from the current request.
/// </summary>
public sealed record UserContext(Guid? UserId, Guid? AthleteId);
