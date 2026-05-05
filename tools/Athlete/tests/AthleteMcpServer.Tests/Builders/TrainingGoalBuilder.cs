using AthleteMcpServer.Domain.Goals;

namespace AthleteMcpServer.Tests.Builders;

public class TrainingGoalBuilder
{
    private Guid _athleteId = Guid.NewGuid();
    private RaceDistance _distance = RaceDistance.HalfMarathon;
    private DateOnly _targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3));
    private TimeSpan? _targetTime = TimeSpan.FromHours(2);
    private string? _description = null;

    public TrainingGoalBuilder ForAthlete(Guid athleteId)
    {
        _athleteId = athleteId;
        return this;
    }

    public TrainingGoalBuilder ForDistance(RaceDistance distance)
    {
        _distance = distance;
        return this;
    }

    public TrainingGoalBuilder WithTargetDate(DateOnly date)
    {
        _targetDate = date;
        return this;
    }

    public TrainingGoalBuilder WithTargetTime(TimeSpan? time)
    {
        _targetTime = time;
        return this;
    }

    public TrainingGoalBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public TrainingGoal Build()
    {
        return new TrainingGoal(
            _athleteId,
            _distance,
            _targetDate,
            _targetTime,
            _description
        );
    }
}
