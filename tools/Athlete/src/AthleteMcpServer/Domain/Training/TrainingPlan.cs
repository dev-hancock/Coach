using System;
using System.Collections.Generic;
using System.Text;

namespace AthleteMcpServer.Domain.TrainingPlans
{

    public sealed class TrainingPlan
    {
        private readonly List<PlannedSession> _sessions = [];

        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid AthleteId { get; private set; }

        public Guid? GoalId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public TrainingPhase Phase { get; private set; } = TrainingPhase.Base;

        public DateOnly StartDate { get; private set; }

        public DateOnly EndDate { get; private set; }

        public decimal TargetWeeklyDistanceKm { get; private set; }

        public int TrainingDaysPerWeek { get; private set; }

        public bool IsActive { get; private set; } = true;

        public IReadOnlyCollection<PlannedSession> Sessions => _sessions;

        private TrainingPlan()
        {
        }

        public TrainingPlan(
            Guid athleteId,
            string name,
            DateOnly startDate,
            DateOnly endDate,
            int trainingDaysPerWeek,
            decimal targetWeeklyDistanceKm,
            Guid? goalId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (endDate < startDate)
            {
                throw new InvalidOperationException("Plan end date cannot be before start date.");
            }

            if (trainingDaysPerWeek is < 1 or > 7)
            {
                throw new InvalidOperationException("Training days per week must be between 1 and 7.");
            }

            AthleteId = athleteId;
            GoalId = goalId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            TrainingDaysPerWeek = trainingDaysPerWeek;
            TargetWeeklyDistanceKm = targetWeeklyDistanceKm;
        }

        public PlannedSession ScheduleSession(
            DateOnly date,
            SessionType sessionType,
            SessionIntensity intensity,
            decimal? targetDistanceKm = null,
            TimeSpan? targetDuration = null,
            TimeSpan? targetPacePerKm = null,
            string? description = null)
        {
            if (date < StartDate || date > EndDate)
            {
                throw new InvalidOperationException("Session must be within the training plan date range.");
            }

            if (targetDistanceKm < 0)
            {
                throw new InvalidOperationException("Target distance cannot be negative.");
            }

            var session = new PlannedSession(
                Id,
                date,
                sessionType,
                intensity,
                targetDistanceKm,
                targetDuration,
                targetPacePerKm,
                description);

            _sessions.Add(session);

            return session;
        }

        public void MoveSession(Guid sessionId, DateOnly newDate)
        {
            var session = GetSession(sessionId);

            if (newDate < StartDate || newDate > EndDate)
            {
                throw new InvalidOperationException("Session must be within the training plan date range.");
            }

            session.MoveTo(newDate);
        }

        public void SkipSession(Guid sessionId, string? reason = null)
        {
            GetSession(sessionId).Skip(reason);
        }

        public void ReplaceSession(
            Guid sessionId,
            SessionType sessionType,
            SessionIntensity intensity,
            decimal? targetDistanceKm,
            TimeSpan? targetDuration,
            TimeSpan? targetPacePerKm,
            string? description)
        {
            GetSession(sessionId).Replace(
                sessionType,
                intensity,
                targetDistanceKm,
                targetDuration,
                targetPacePerKm,
                description);
        }

        public void LinkCompletedRun(Guid sessionId, Guid completedRunId)
        {
            GetSession(sessionId).Complete(completedRunId);
        }

        public void Archive()
        {
            IsActive = false;
        }

        private PlannedSession GetSession(Guid sessionId)
        {
            return _sessions.SingleOrDefault(x => x.Id == sessionId)
                ?? throw new InvalidOperationException("Planned session was not found.");
        }
    }
}
