using Coach.Domain.Common;

namespace Coach.Domain.Gear;

/// <summary>
///     Equipment aggregate root representing tracked athletic gear for an athlete.
/// </summary>
public sealed class Equipment : AggregateRoot
{
    private Equipment()
    {
    }

    public Equipment(
        Guid athleteId,
        EquipmentType type,
        string name,
        string? brand = null,
        string? model = null,
        DateOnly? firstUsedOn = null,
        decimal? retireAfterKm = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (retireAfterKm.HasValue && retireAfterKm.Value <= 0)
        {
            throw new InvalidOperationException("Retirement distance must be greater than zero.");
        }

        AthleteId = athleteId;
        Type = type;
        Name = name;
        Brand = brand;
        Model = model;
        FirstUsedOn = firstUsedOn;
        RetireAfterKm = retireAfterKm ?? GetDefaultRetirementDistance(type);
    }

    public Guid AthleteId { get; private set; }

    public EquipmentType Type { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Brand { get; private set; }

    public string? Model { get; private set; }

    public DateOnly? FirstUsedOn { get; private set; }

    public decimal? RetireAfterKm { get; private set; }

    public decimal DistanceLoggedKm { get; private set; }

    public bool IsRetired { get; private set; }

    public void AddMileage(decimal distanceKm)
    {
        if (distanceKm <= 0)
        {
            throw new InvalidOperationException("Distance must be greater than zero.");
        }

        DistanceLoggedKm += distanceKm;
    }

    public void Retire()
    {
        IsRetired = true;
    }

    public void UpdateDetails(
        string name,
        string? brand = null,
        string? model = null,
        decimal? retireAfterKm = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (retireAfterKm.HasValue && retireAfterKm.Value <= 0)
        {
            throw new InvalidOperationException("Retirement distance must be greater than zero.");
        }

        Name = name;
        Brand = brand;
        Model = model;

        if (retireAfterKm.HasValue)
        {
            RetireAfterKm = retireAfterKm.Value;
        }
    }

    private static decimal GetDefaultRetirementDistance(EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Shoe => 650m,
            EquipmentType.Bike => 10000m,
            _ => 1000m
        };
    }
}