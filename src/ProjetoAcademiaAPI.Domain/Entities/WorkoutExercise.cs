using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public sealed class WorkoutExercise : EntityBase, IUserOwned
    {
        private readonly List<PlannedSet> _plannedSets = new List<PlannedSet>();

        public Guid UserId { get; private set; }
        public long WorkoutId { get; private set; }
        public long ExerciseId { get; private set; }
        public int Order { get; private set; }
        public int? RestSeconds { get; private set; }

        public string? Observations { get; private set; } = null;

        //Propriedade de navegação
        public Exercise Exercise { get; private set; } = null!;

        public IReadOnlyCollection<PlannedSet> PlannedSets => _plannedSets;

        //Cria um construtor privado para impedir a criação de instâncias da classe WorkoutExcercise fora dela.
        private WorkoutExercise() { }

        //Cria um construtor público que inicializa as propriedades da classe WorkoutExcercise.
        internal WorkoutExercise(Guid userId, long workoutId, long exerciseId, int order, int? restSeconds, string? observations)
        {
            //Valida se o tempo de descanso é negativo, lançando uma exceção se for o caso.
            if (restSeconds is not null && restSeconds < 0)
            {
                throw new ArgumentException(Messages.RestTimeSecondsCannotBeNegative);
            }
            UserId = userId;
            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            Order = order;
            RestSeconds = restSeconds;
            Observations = observations;
        }

        public void AddPlannedSet(int quantity, SetType setType, int targetRepsMin, 
                                  int? targetRepsMax, int[] dropTargetReps)
        {

            //Refazendo a ordem dos sets planejados antes de adicionar um novo set, para garantir que a ordem esteja correta.
            var orderedSets = _plannedSets.OrderBy(set => set.Order).ToArray();

            for (var i = 0; i < orderedSets.Length; i++)
            {
                orderedSets[i].SetOrder(i + 1);
            }

            //Adiciona um novo set planejado à lista de sets planejados.
            _plannedSets.Add(new PlannedSet(UserId,
                                            Id,
                                            _plannedSets.Max(o => o.Order) + 1,
                                            quantity,
                                            setType,
                                            targetRepsMin,
                                            targetRepsMax,
                                            dropTargetReps));
        }

        internal void SetOrder(int order)
        {
            Order = order;
        }
    }
}
