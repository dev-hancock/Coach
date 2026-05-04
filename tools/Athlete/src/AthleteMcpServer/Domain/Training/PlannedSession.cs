using System;
using System.Collections.Generic;
using System.Text;

namespace AthleteMcpServer.Domain.TrainingPlans
{

    public sealed class PlannedSession
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid TrainingPlanId { get; private set; }

        public DateOnly Date { get; private set; }

        public SessionType SessionType { get; private set; }

        public SessionIntensity Intensity { get; private set; }

        public PlannedSessionStatus Status { get; private set; } = PlannedSessionStatus.Planned;

        public decimal? TargetDistanceKm { get; private set; }

        public TimeSpan? TargetDuration { get; private set; }

        public TimeSpan? TargetPacePerKm { get; private set; }

        public string? Description { get; private set; }

        public Guid? CompletedRunId { get; private set; }

        private PlannedSession()
        {
        }

        internal PlannedSession(
            Guid trainingPlanId,
            DateOnly date,
            SessionType sessionType,
            SessionIntensity intensity,
            decimal? targetDistanceKm,
            TimeSpan? targetDuration,
            TimeSpan? targetPacePerKm,
            string? description)
        {
            TrainingPlanId = trainingPlanId;
            Date = date;
            SessionType = sessionType;
            Intensity = intensity;
            TargetDistanceKm = targetDistanceKm;
            TargetDuration = targetDuration;
            TargetPacePerKm = targetPacePerKm;
            Description = description;
        }

        internal void MoveTo(DateOnly newDate)
        {
            Date = newDate;
            Status = PlannedSessionStatus.Moved;
        }

        internal void Skip(string? reason)
        {
            Status = PlannedSessionStatus.Skipped;
            Description = string.IsNullOrWhiteSpace(reason)
                ? Description
                : $"{Description}{Environment.NewLine}Skipped: {reason}".Trim();
        }

        internal void Replace(
            SessionType sessionType,
            SessionIntensity intensity,
            decimal? targetDistanceKm,
            TimeSpan? targetDuration,
            TimeSpan? targetPacePerKm,
            string? description)
        {
            SessionType = sessionType;
            Intensity = intensity;
            TargetDistanceKm = targetDistanceKm;
            TargetDuration = targetDuration;
            TargetPacePerKm = targetPacePerKm;
            Description = description;
            Status = PlannedSessionStatus.Replaced;
        }

        internal void Complete(Guid completedRunId)
        {
            if (Status == PlannedSessionStatus.Completed)
            {
                throw new InvalidOperationException("Session is already completed.");
            }

            CompletedRunId = completedRunId;
            Status = PlannedSessionStatus.Completed;
        }
    }
}
