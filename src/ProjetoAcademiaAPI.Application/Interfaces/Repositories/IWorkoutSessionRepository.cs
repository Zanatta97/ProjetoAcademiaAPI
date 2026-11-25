using ProjetoAcademiaAPI.Application.Features.PersonalRecords;
using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    public interface IWorkoutSessionRepository : IRepository<WorkoutSession>
    {
        public Task<WorkoutSession?> GetByClientIdAsync(Guid clientId, CancellationToken ct);

        Task<IReadOnlyDictionary<long, ExerciseBest>> GetBestSetsAsync(IEnumerable<long> exercisesIds, CancellationToken ct);
    }
}
