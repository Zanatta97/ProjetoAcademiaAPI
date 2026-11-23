using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Enums;
using ProjetoAcademiaAPI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public class WorkoutSession : EntityBase, IUserOwned
    {
        private readonly List<SessionSet> _sets = [];
        public Guid UserId { get; private set; }
        // ID do cliente associado à sessão de treino.
        public Guid ClientId { get; private set; }
        public long WorkoutId { get; private set; }
        //Usado DateTimeOffset para armazenar a data e hora de início da sessão de treino, incluindo o fuso horário.
        public DateTimeOffset StartedAt { get; private set; } 
        public DateTimeOffset FinishedAt { get; private set; }
        public int DurationSeconds { get; private set; }
        public decimal TotalVolumeKg { get; private set; }

        private WorkoutSession() { }
        
        public static WorkoutSession Register(Guid userId,
                                              Guid clientId,
                                              long workoutId,
                                              DateTimeOffset startedAt,
                                              DateTimeOffset finishedAt)
        {

            if (startedAt > finishedAt)
            {
                throw new DomainException(Messages.StartTimeCannotBeAfterEndTime);
            }


            return new WorkoutSession
            {
                UserId = userId,
                ClientId = clientId,
                WorkoutId = workoutId,
                StartedAt = startedAt,
                FinishedAt = finishedAt,
                DurationSeconds = (int)(finishedAt - startedAt).TotalSeconds
            };
        }

        public SessionSet AddSet(long exerciseId, long? plannedSetId, SetType setType,
                                 decimal weightKg, int reps, DateTimeOffset completedAt)
        {
            var set = new SessionSet(UserId, Id, exerciseId, plannedSetId ?? 0, _sets.Count + 1, weightKg, reps, completedAt);

            _sets.Add(set);

            if (setType.CountAsVolume())
                TotalVolumeKg += weightKg * reps;

            return set;
        }
    }
}
