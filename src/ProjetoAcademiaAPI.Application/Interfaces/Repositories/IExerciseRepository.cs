using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        /// <summary>
        /// Valida os Ids retornados do client na sessão.
        /// </summary>
        /// <param name="exerciseIds">Ids dos exercícios realizados</param>
        /// <param name="ct">Token de cancelamento</param>
        public Task<bool> AllExistsAsync(IEnumerable<long> exerciseIds, CancellationToken ct);

    }
}
