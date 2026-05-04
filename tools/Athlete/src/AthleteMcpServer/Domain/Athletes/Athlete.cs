using System;
using System.Collections.Generic;
using System.Text;

namespace AthleteMcpServer.Domain.Athletes
{

    public sealed class Athlete
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; } = string.Empty;

        public ExperienceLevel ExperienceLevel { get; private set; } = ExperienceLevel.Beginner;

        public UnitSystem PreferredUnits { get; private set; } = UnitSystem.Metric;

        public int TrainingDaysPerWeek { get; private set; }

        public DayOfWeek? PreferredLongRunDay { get; private set; }

        public decimal CurrentWeeklyDistanceKm { get; private set; }

        public decimal TypicalLongRunDistanceKm { get; private set; }

        public string? Notes { get; private set; }

        private Athlete()
        {
        }

        public Athlete(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
        }

        public void UpdateProfile(
            ExperienceLevel experienceLevel,
            UnitSystem preferredUnits,
            int trainingDaysPerWeek,
            DayOfWeek? preferredLongRunDay,
            decimal currentWeeklyDistanceKm,
            decimal typicalLongRunDistanceKm,
            string? notes = null)
        {
            if (trainingDaysPerWeek is < 1 or > 7)
            {
                throw new InvalidOperationException("Training days per week must be between 1 and 7.");
            }

            if (currentWeeklyDistanceKm < 0 || typicalLongRunDistanceKm < 0)
            {
                throw new InvalidOperationException("Distances cannot be negative.");
            }

            ExperienceLevel = experienceLevel;
            PreferredUnits = preferredUnits;
            TrainingDaysPerWeek = trainingDaysPerWeek;
            PreferredLongRunDay = preferredLongRunDay;
            CurrentWeeklyDistanceKm = currentWeeklyDistanceKm;
            TypicalLongRunDistanceKm = typicalLongRunDistanceKm;
            Notes = notes;
        }
    }
}
