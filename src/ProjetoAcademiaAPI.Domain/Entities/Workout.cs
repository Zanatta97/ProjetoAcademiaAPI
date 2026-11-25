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
        private readonly List<WorkoutExercise> _exercises = new List<WorkoutExercise>(); 
        public Guid UserId { get; private set; }

        // O null! é usado para indicar que a propriedade não pode ser nula, mas ainda não foi inicializada.
        public string Name { get; private set; } = null!;
        public string? Observations { get; private set; } = null;
        public DaysOfWeek DaysOfWeek { get; private set; }
        public bool IsArchived { get; private set; }
        public IReadOnlyCollection<WorkoutExercise> Exercises => _exercises;

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

        /// <summary>
        /// Adiciona um exercício ao treino, garantindo que a ordem dos exercícios existentes
        /// seja atualizada antes da adição do novo exercício.
        /// </summary>
        /// <param name="exerciseId">O identificador do exercício a ser adicionado.</param>
        /// <param name="restSeconds">O tempo de descanso em segundos, ou null se nenhum descanso for especificado.</param>
        /// <returns>Retorna o exercício adicionado.</returns>
        public WorkoutExercise AddExercise(long exerciseId, int? restSeconds)
        {
            //Refazendo a ordem dos exercicios antes de adicionar um novo, para garantir que a ordem esteja correta.
            var orderedExercises = _exercises.OrderBy(e => e.Order).ToArray();

            for (var i = 0; i < orderedExercises.Length; i++)
            {
                orderedExercises[i].SetOrder(i + 1);
            }

            var item = new WorkoutExercise(UserId, Id, exerciseId, _exercises.Count + 1, restSeconds, null);
            _exercises.Add(item);
            return item;
        }

        /// <summary>
        /// Remove todos os exercícios associados ao treino, limpando a lista de exercícios.
        /// </summary>
        public void ClearExercises()
        {
            _exercises.Clear();
        }

        /// <summary>
        /// Marca o treino como arquivado ou desarquivado, alternando o estado da propriedade IsArchived.
        /// </summary>
        public void Archive()
        {
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
