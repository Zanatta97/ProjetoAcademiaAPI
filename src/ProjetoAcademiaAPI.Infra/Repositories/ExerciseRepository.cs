using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Determina se todos os exercícios especificados existem no banco de dados.
        /// </summary>
        /// <param name="exerciseIds">Uma lista de IDs de exercícios a serem verificados.</param>
        /// <param name="ct">O token de cancelamento a ser monitorado.</param>
        /// <returns>Verdadeiro se todos os exercícios existirem; caso contrário, falso.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando `exerciseIds` é `null`.</exception>
        public async Task<bool> AllExistsAsync(IEnumerable<long> exerciseIds, CancellationToken ct)
        {
            var ids = exerciseIds.Distinct().ToList();

            if (ids.Count == 0)
            {
                return true;
            }

            var found = await _context.Set<Exercise>().CountAsync(e => ids.Contains(e.Id), ct);
            return found == ids.Count;
        }
    }
}
