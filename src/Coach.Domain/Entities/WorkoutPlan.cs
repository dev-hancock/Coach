using Coach.Domain.Common;

namespace Coach.Domain.Entities;

/// <summary>
///     Workout plan aggregate root.
/// </summary>
public sealed class WorkoutPlan : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}