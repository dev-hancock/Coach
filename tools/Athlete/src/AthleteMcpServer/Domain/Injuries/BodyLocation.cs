namespace AthleteMcpServer.Domain.Injuries;

/// <summary>
/// Structured body location for injury tracking.
/// </summary>
public enum BodyLocation
{
    // Foot
    Foot,
    Toes,
    Arch,
    Heel,
    Ankle,

    // Lower Leg
    Calf,
    Shin,
    Achilles,

    // Knee
    Knee,
    KneeCap,

    // Upper Leg
    Hamstring,
    Quadriceps,
    ITBand,
    Groin,
    Hip,

    // Core
    LowerBack,
    UpperBack,
    Abdominals,

    // Other
    Shoulder,
    Neck,
    Other
}
