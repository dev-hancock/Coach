namespace Coach.Domain.Common;

/// <summary>
///     Base class for value objects - immutable objects defined by their properties, not identity.
/// </summary>
public abstract record ValueObject
{
    // Records in C# provide automatic value-based equality,
    // which is exactly what we need for value objects.
    // No need to override Equals/GetHashCode manually.
}