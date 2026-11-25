using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public sealed class Exercise : EntityBase
    {
        public Guid? UserId { get; private set; }
        public string Name { get; private set; } = null!;
        public MuscleGroup MucleGroup { get; private set; }
        public bool IsArchived { get; private set; }
        public string? Observations { get; private set; } = null;
        public bool IsGlobal => UserId == null;

        private Exercise() { }

        public Exercise(Guid? userId, string name, MuscleGroup muscleGroup, string? observations)
        {
            UserId = userId;  
            Name = name;
            MucleGroup = muscleGroup;
            Observations = observations;
            IsArchived = false;
        }

        public void Update(string name, MuscleGroup muscleGroup, string observations)
        {
            if (IsGlobal)
            {
                throw new InvalidOperationException(Messages.UpdateGlobalExerciseNotAllowed);
            }

            Name = name;
            MucleGroup = muscleGroup;
            Observations = observations;
        }

        public void AlterArchived()
        {
            if (IsGlobal)
            {
                throw new InvalidOperationException(Messages.UpdateGlobalExerciseNotAllowed);
            }

            if (IsArchived)
            {
                IsArchived = false;
            }
            else
            {
                IsArchived = true;
            }

        }

    }
}
