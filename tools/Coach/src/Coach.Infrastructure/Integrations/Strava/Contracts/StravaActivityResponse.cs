using System.Text.Json.Serialization;

namespace Coach.Infrastructure.Integrations.Strava.Contracts;

public record StravaActivityResponse(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("start_date")] DateTime StartDate,
    [property: JsonPropertyName("distance")] decimal Distance,
    [property: JsonPropertyName("moving_time")] int MovingTime,
    [property: JsonPropertyName("elapsed_time")] int ElapsedTime,
    [property: JsonPropertyName("average_heartrate")] decimal? AverageHeartrate,
    [property: JsonPropertyName("max_heartrate")] decimal? MaxHeartrate);