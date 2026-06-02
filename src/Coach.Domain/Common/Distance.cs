namespace Coach.Domain.Common;

/// <summary>
/// Value object representing a distance measurement.
/// </summary>
public sealed record Distance : ValueObject
{
    public decimal Kilometers { get; }

    private Distance(decimal kilometers)
    {
        Kilometers = kilometers;
    }

    public static Distance FromKilometers(decimal kilometers)
    {
        if (kilometers < 0)
        {
            throw new ArgumentException("Distance cannot be negative.", nameof(kilometers));
        }

        return new Distance(kilometers);
    }

    public static Distance FromMiles(decimal miles)
    {
        if (miles < 0)
        {
            throw new ArgumentException("Distance cannot be negative.", nameof(miles));
        }

        const decimal milesPerKilometer = 0.621371m;

        return new Distance(miles / milesPerKilometer);
    }

    public static Distance Zero => new(0);

    public decimal ToMiles() => Kilometers * 0.621371m;

    public static Distance operator +(Distance left, Distance right)
        => new(left.Kilometers + right.Kilometers);

    public static Distance operator -(Distance left, Distance right)
    {
        var result = left.Kilometers - right.Kilometers;
        if (result < 0)
        {
            throw new InvalidOperationException("Distance cannot be negative.");
        }
        return new(result);
    }

    public static bool operator >(Distance left, Distance right)
        => left.Kilometers > right.Kilometers;

    public static bool operator <(Distance left, Distance right)
        => left.Kilometers < right.Kilometers;

    public static bool operator >=(Distance left, Distance right)
        => left.Kilometers >= right.Kilometers;

    public static bool operator <=(Distance left, Distance right)
        => left.Kilometers <= right.Kilometers;

    public override string ToString() => $"{Kilometers:F2} km";
}
