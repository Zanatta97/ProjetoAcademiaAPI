using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class ApiLogRepository : Repository<ApiLog>, IApiLogRepository
    {
        public ApiLogRepository(AppDbContext context) : base(context)
        {
        }

        public void AddRange(IEnumerable<ApiLog> logs)
        {
            _context.Set<ApiLog>().AddRange(logs);
        }

        /// <summary>
        /// Deleta todos os registros de log que ocorreram antes do tempo de corte fornecido de forma assíncrona.
        /// </summary>
        /// <param name="cutoff">O tempo de corte. Logs que ocorreram antes deste tempo são excluídos.</param>
        /// <param name="ct">O token usado para cancelar a operação de salvamento assíncrona.</param>
        /// <returns>Retorna o número de registros excluídos.</returns>
        public Task<int> DeleteOlderThanAsync(DateTimeOffset cutoff, CancellationToken ct)
        {
            return _context.Set<ApiLog>().Where(l => l.OcurredAt < cutoff).ExecuteDeleteAsync(ct);
        }
    }
}
