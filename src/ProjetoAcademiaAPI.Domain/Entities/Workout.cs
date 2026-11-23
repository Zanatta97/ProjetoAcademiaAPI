using ProjetoAcademiaAPI.Domain.Common;
using ProjetoAcademiaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Domain.Entities
{
    public sealed class Workout : EntityBase, IUserOwned
    {
        //Representa a lista de exercícios associados ao treino.
        //É inicializada como uma lista vazia e é privada para garantir que
        //apenas a classe Workout possa modificar essa lista diretamente.
        private readonly List<WorkoutExcercise> _exercises = new List<WorkoutExcercise>(); 
        public Guid UserId { get; private set; }

        // O null! é usado para indicar que a propriedade não pode ser nula, mas ainda não foi inicializada.
        public string Name { get; private set; } = null!;
        public string? Observations { get; private set; } = null;
        public DaysOfWeek DaysOfWeek { get; private set; }
        public bool IsArchived { get; private set; }
        public IReadOnlyCollection<WorkoutExcercise> Exercises => _exercises;

        //Cria um construtor privado para impedir a criação de instâncias da classe Workout fora dela.
        private Workout() { }

        //Cria um construtor público que inicializa as propriedades da classe Workout.
        public Workout(Guid userId, string name, DaysOfWeek daysOfWeek, string? observations)
        {
            UserId = userId;
            Name = name;
            DaysOfWeek = daysOfWeek;
            Observations = observations;
            IsArchived = false;
        } 
    }
}
