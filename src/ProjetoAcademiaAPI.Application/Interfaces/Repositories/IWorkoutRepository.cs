using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    public interface IWorkoutRepository : IRepository<Workout>
    {
        /// <summary>
        /// Retorna apenas os exercícios ativos.
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public Task<IReadOnlyList<Workout>> ListActiveAsync(CancellationToken ct);
    }
}
