namespace Coach.Domain.Activities;

/// <summary>
///     Source of activity data import.
/// </summary>
public enum ActivitySource
{
    Manual = 1, // Manually logged
    FitFile = 2, // Generic .fit file upload
    Garmin = 3, // Garmin Connect API
    Strava = 4, // Strava API
    Coros = 5, // Coros API
    Polar = 6, // Polar Flow API
    Suunto = 7, // Suunto API
    Wahoo = 8 // Wahoo API
}