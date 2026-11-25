using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Features.PersonalRecords
{
    public sealed record ExerciseBest
    {
        public long ExerciseId { get; init; }
        public decimal WeightKg { get; init; }
        public int Reps { get; init; }

        public ExerciseBest(long exerciseId, decimal weightKg, int reps)
        {
            ExerciseId = exerciseId;
            WeightKg = weightKg;
            Reps = reps;
        }
    }
}
