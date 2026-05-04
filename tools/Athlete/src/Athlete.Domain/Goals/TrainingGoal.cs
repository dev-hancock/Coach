using System;
using System.Collections.Generic;
using System.Text;

namespace Athlete.Domain.Goals
{

    public sealed class TrainingGoal
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid AthleteId { get; private set; }

        public RaceDistance Distance { get; private set; }

        public DateOnly TargetDate { get; private set; }

        public TimeSpan? TargetTime { get; private set; }

        public string? Description { get; private set; }

        public GoalStatus Status { get; private set; } = GoalStatus.Active;

        private TrainingGoal()
        {
        }

        public TrainingGoal(
            Guid athleteId,
            RaceDistance distance,
            DateOnly targetDate,
            TimeSpan? targetTime = null,
            string? description = null)
        {
            AthleteId = athleteId;
            Distance = distance;
            TargetDate = targetDate;
            TargetTime = targetTime;
            Description = description;
        }

        public void ChangeTarget(DateOnly targetDate, TimeSpan? targetTime)
        {
            if (Status != GoalStatus.Active)
            {
                throw new InvalidOperationException("Only active goals can be changed.");
            }

            TargetDate = targetDate;
            TargetTime = targetTime;
        }

        public void Complete() => Status = GoalStatus.Completed;

        public void Abandon() => Status = GoalStatus.Abandoned;
    }
}
