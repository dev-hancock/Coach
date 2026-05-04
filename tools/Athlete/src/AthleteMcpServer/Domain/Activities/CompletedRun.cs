using System;
using System.Collections.Generic;
using System.Text;

namespace AthleteMcpServer.Domain.Activities
{

    public sealed class CompletedRun
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid AthleteId { get; private set; }

        public Guid? PlannedSessionId { get; private set; }

        public Guid? ShoeId { get; private set; }

        public DateTimeOffset StartedAt { get; private set; }

        public decimal DistanceKm { get; private set; }

        public TimeSpan Duration { get; private set; }

        public TimeSpan? MovingTime { get; private set; }

        public TimeSpan? AveragePacePerKm { get; private set; }

        public int? AverageHeartRate { get; private set; }

        public int? MaxHeartRate { get; private set; }

        public int? AverageCadence { get; private set; }

        public decimal? ElevationGainMeters { get; private set; }

        public int? RatePerceivedExertion { get; private set; }

        public RunSource Source { get; private set; } = RunSource.Manual;

        public string? ExternalActivityId { get; private set; }

        public string? Notes { get; private set; }

        private CompletedRun()
        {
        }

        public CompletedRun(
            Guid athleteId,
            DateTimeOffset startedAt,
            decimal distanceKm,
            TimeSpan duration,
            int? ratePerceivedExertion = null,
            RunSource source = RunSource.Manual)
        {
            if (distanceKm <= 0)
            {
                throw new InvalidOperationException("Run distance must be greater than zero.");
            }

            if (duration <= TimeSpan.Zero)
            {
                throw new InvalidOperationException("Run duration must be greater than zero.");
            }

            if (ratePerceivedExertion is < 1 or > 10)
            {
                throw new InvalidOperationException("RPE must be between 1 and 10.");
            }

            AthleteId = athleteId;
            StartedAt = startedAt;
            DistanceKm = distanceKm;
            Duration = duration;
            RatePerceivedExertion = ratePerceivedExertion;
            Source = source;
            AveragePacePerKm = TimeSpan.FromSeconds(duration.TotalSeconds / (double)distanceKm);
        }

        public void AttachToPlannedSession(Guid plannedSessionId)
        {
            PlannedSessionId = plannedSessionId;
        }

        public void AssignShoe(Guid shoeId)
        {
            ShoeId = shoeId;
        }

        public void AddMetrics(
            TimeSpan? movingTime,
            int? averageHeartRate,
            int? maxHeartRate,
            int? averageCadence,
            decimal? elevationGainMeters)
        {
            MovingTime = movingTime;
            AverageHeartRate = averageHeartRate;
            MaxHeartRate = maxHeartRate;
            AverageCadence = averageCadence;
            ElevationGainMeters = elevationGainMeters;
        }

        public void AddExternalReference(RunSource source, string externalActivityId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(externalActivityId);

            Source = source;
            ExternalActivityId = externalActivityId;
        }

        public void AddNotes(string notes)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(notes);
            Notes = notes;
        }
    }
}
