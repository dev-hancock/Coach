using Coach.Application.Abstractions.Integrations;
using Coach.Application.Abstractions.Integrations.Strava;
using ErrorOr;

namespace Coach.Infrastructure.Integrations.Strava;

/// <summary>
/// Implementation of Strava activity service using Refit.
/// </summary>
internal sealed class StravaActivityService(IStravaApi api) : IStravaActivityService
{
    public async Task<ErrorOr<List<StravaActivityData>>> GetActivitiesAsync(
        string accessToken,
        DateTime? after = null,
        int pageSize = 30,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var afterTimestamp = after.HasValue
                ? new DateTimeOffset(after.Value).ToUnixTimeSeconds()
                : (long?)null;

            var response = await api.GetActivitiesAsync(
                $"Bearer {accessToken}",
                afterTimestamp,
                pageSize);

            var activities = response
                .Select(a => new StravaActivityData(
                    a.Id,
                    a.Name,
                    a.Type,
                    a.StartDate,
                    a.Distance,
                    a.MovingTime,
                    a.ElapsedTime,
                    a.AverageHeartrate,
                    a.MaxHeartrate))
                .ToList();

            return activities;
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.GetActivities",
                $"Failed to get activities: {ex.Message}");
        }
    }

    public async Task<ErrorOr<StravaActivityData>> GetActivityAsync(
        string accessToken,
        long activityId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await api.GetActivityAsync(
                $"Bearer {accessToken}",
                activityId);

            return new StravaActivityData(
                response.Id,
                response.Name,
                response.Type,
                response.StartDate,
                response.Distance,
                response.MovingTime,
                response.ElapsedTime,
                response.AverageHeartrate,
                response.MaxHeartrate);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                "Strava.GetActivity",
                $"Failed to get activity: {ex.Message}");
        }
    }
}
