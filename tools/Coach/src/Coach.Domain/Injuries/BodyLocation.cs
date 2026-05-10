using Coach.Domain.Common;

namespace Coach.Domain.Injuries;

/// <summary>
///     Body location entity representing configurable injury locations per discipline.
///     Reference data entity allowing runtime extensibility.
/// </summary>
public sealed class BodyLocation : Entity
{
    private BodyLocation()
    {
    }

    public BodyLocation(
        string name,
        BodyRegion region,
        Discipline discipline = Discipline.All,
        bool isCommon = false,
        Laterality? side = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Region = region;
        Discipline = discipline;
        IsCommon = isCommon;
        Side = side;
    }

    /// <summary>
    ///     Display name of the location (e.g., "Left Achilles", "Right Knee", "Lower Back").
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    ///     General body region for grouping and analytics.
    /// </summary>
    public BodyRegion Region { get; private set; }

    /// <summary>
    ///     Specific discipline where this location is most commonly injured.
    ///     Null/All means applicable to all disciplines.
    /// </summary>
    public Discipline Discipline { get; private set; }

    /// <summary>
    ///     Whether this is a commonly tracked location (for UI prioritization).
    /// </summary>
    public bool IsCommon { get; private set; }

    /// <summary>
    ///     Optional laterality - Left, Right, or null for central locations.
    /// </summary>
    public Laterality? Side { get; private set; }

    public void UpdateDetails(
        string name,
        BodyRegion region,
        Discipline discipline,
        bool isCommon)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Region = region;
        Discipline = discipline;
        IsCommon = isCommon;
    }
}

/// <summary>
///     Body laterality for bilateral locations.
/// </summary>
public enum Laterality
{
    Left = 1,
    Right = 2
}