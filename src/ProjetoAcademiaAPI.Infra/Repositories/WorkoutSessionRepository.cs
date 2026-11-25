using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Features.PersonalRecords;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Domain.Enums;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class WorkoutSessionRepository : Repository<WorkoutSession>, IWorkoutSessionRepository
    {
        public WorkoutSessionRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retorna o melhor conjunto de cada exercício, ordenado por peso e depois repetições.
        /// Retorna um dicionário vazio quando não há identificadores de exercícios fornecidos.
        /// </summary>
        /// <param name="exercisesIds">Os identificadores dos exercícios para os quais buscar os melhores conjuntos.</param>
        /// <param name="ct">O token usado para cancelar a operação assíncrona.</param>
        /// <returns>Apenas os melhores conjuntos para cada exercício dos sets de tipo Working ou Drop.</returns>
        public async Task<IReadOnlyDictionary<long, ExerciseBest>> GetBestSetsAsync(IEnumerable<long> exercisesIds, CancellationToken ct)
        {
            var ids = exercisesIds.Distinct().ToList();

            if (ids.Count == 0)
            {
                return new Dictionary<long, ExerciseBest>();
            }

            var bests = await _context.Set<SessionSet>()
                .AsNoTracking()
                .Where(s => ids.Contains(s.ExerciseId) && (s.SetType == SetType.Working || s.SetType == SetType.Drop))
                .GroupBy(s => s.ExerciseId)
                .Select(g => g
                    .OrderByDescending(s => s.WeightKg)
                    .ThenByDescending(s => s.Reps)
                    .Select(s => new {s.ExerciseId, s.WeightKg, s.Reps})
                    .First())
                .ToListAsync(ct);

            return bests.ToDictionary(b => b.ExerciseId, b => new ExerciseBest(b.ExerciseId,b.WeightKg, b.Reps));
        }

        /// <summary>
        /// Busca a sessão pelo ClientId gerado no app, com as séries. Usado na idempotência:
        /// um reenvio devolve o que já foi gravado.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<WorkoutSession?> GetByClientIdAsync(Guid clientId, CancellationToken ct)
        {
            return await _context.Set<WorkoutSession>().AsNoTracking()
                .Include(s => s.Sets.OrderBy(set => set.Order))
                .FirstOrDefaultAsync(s => s.ClientId == clientId, ct);
        }
    }
}
