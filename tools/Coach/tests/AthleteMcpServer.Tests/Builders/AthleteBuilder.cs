namespace AthleteMcpServer.Tests.Builders;

public class AthleteBuilder
{
    private string _name = "Test Runner";
    private ExperienceLevel _experienceLevel = ExperienceLevel.Intermediate;
    private UnitSystem _preferredUnits = UnitSystem.Metric;
    private int _trainingDaysPerWeek = 4;
    private DayOfWeek? _preferredLongRunDay = DayOfWeek.Sunday;
    private decimal _currentWeeklyDistanceKm = 30m;
    private decimal _typicalLongRunDistanceKm = 15m;
    private string? _notes = null;

    public AthleteBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AthleteBuilder WithExperienceLevel(ExperienceLevel level)
    {
        _experienceLevel = level;
        return this;
    }

    public AthleteBuilder WithPreferredUnits(UnitSystem units)
    {
        _preferredUnits = units;
        return this;
    }

    public AthleteBuilder WithTrainingDaysPerWeek(int days)
    {
        _trainingDaysPerWeek = days;
        return this;
    }

    public AthleteBuilder WithCurrentWeeklyDistance(decimal km)
    {
        _currentWeeklyDistanceKm = km;
        return this;
    }

    public Athlete Build()
    {
        var athlete = new Athlete(_name);

        // Update profile with additional details
        athlete.UpdateProfile(
            _experienceLevel,
            _preferredUnits,
            _trainingDaysPerWeek,
            _preferredLongRunDay,
            _currentWeeklyDistanceKm,
            _typicalLongRunDistanceKm,
            _notes
        );

        return athlete;
    }
}
