namespace Coach.Domain.Activities;

/// <summary>
///     Type of athletic activity, supporting triathlon and functional fitness disciplines.
/// </summary>
public enum ActivityType
{
    Run = 1,
    Ride = 2, // Cycling (Strava naming)
    Swim = 3,
    Strength = 4, // Gym/functional fitness
    Workout = 5, // Generic workout (future Hyrox)
    Other = 99
}