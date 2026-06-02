using Coach.Application.Abstractions.Identity;
using ErrorOr;
using MediatR;

namespace Coach.Application.Features.Athletes.GetProfile;

/// <summary>
/// Query to get an athlete's profile.
/// </summary>
public sealed record GetAthleteRequest(UserContext Context) : IRequest<ErrorOr<GetAthleteResponse>>;
