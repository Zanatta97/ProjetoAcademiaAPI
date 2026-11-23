using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Enums;
using ProjetoAcademiaAPI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public sealed class PlannedSet : EntityBase, IUserOwned
    {
        public Guid UserId { get; private set; }
        public long WorkoutExcerciseId { get; private set; }
        public int Order { get; private set; }
        public int Quantity { get; private set; }
        public SetType SetType { get; private set; }
        public int? TargetRepsMin { get; private set; }
        public int? TargetRepsMax { get; private set; }

        public int[] DropTargetReps { get; private set; } = [];

        public bool IsRange => TargetRepsMax is not null && TargetRepsMax > 0;

        private PlannedSet() { }

        internal PlannedSet(Guid userId, long workoutExcerciseId, 
                            int order, int quantity, SetType setType, 
                            int? targetRepsMin, int? targetRepsMax, int[] dropTargetReps)
        {

            if (targetRepsMax is not null && targetRepsMax < targetRepsMin)
            {
                throw new DomainException(Messages.MaxRepsCannotBeLessThanMinReps);
            }

            if (setType == SetType.Drop && (dropTargetReps is null || dropTargetReps.Length == 0))
            {
                throw new DomainException(Messages.DropSetRequiresDropTargetReps);
            }

            UserId = userId;
            WorkoutExcerciseId = workoutExcerciseId;
            Order = order;
            Quantity = quantity;
            SetType = setType;
            TargetRepsMin = targetRepsMin;
            TargetRepsMax = targetRepsMax;
            DropTargetReps = dropTargetReps ?? [];
        }

        internal void SetOrder(int order)
        {
            Order = order;
        }
    }
}
