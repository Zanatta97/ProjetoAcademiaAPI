using ProjetoAcademiaAPI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public class SessionSet : EntityBase, IUserOwned
    {
        public Guid UserId { get; private set; }
        public long WorkoutSessionId { get; private set; }
        public long ExerciseId { get; private set; }
        public long PlannedSetId { get; private set; }
        public int Order { get; private set; }
        public decimal WeightKg { get; private set; }
        public int Reps { get; private set; }
        public DateTimeOffset CompletedAt { get; private set; }
        public bool IsPersonalRecord { get; private set; }

        private SessionSet() { }

        internal SessionSet(Guid userId, long workoutSessionId, long exerciseId,
                            long plannedSetId, int order, decimal weightKg, 
                            int reps, DateTimeOffset completedAt)
        {
            UserId = userId;
            WorkoutSessionId = workoutSessionId;
            ExerciseId = exerciseId;
            PlannedSetId = plannedSetId;
            Order = order;
            WeightKg = weightKg;
            Reps = reps;
            CompletedAt = completedAt;
        }

        public void MarkAsPersonalRecord()
        {
            IsPersonalRecord = true;
        }

    }
}
