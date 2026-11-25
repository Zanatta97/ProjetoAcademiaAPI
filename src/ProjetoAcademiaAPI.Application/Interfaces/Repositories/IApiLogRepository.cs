using ProjetoAcademiaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Interfaces.Repositories
{
    public interface IApiLogRepository : IRepository<ApiLog>
    {
        public void AddRange(IEnumerable<ApiLog> logs);
        public Task<int> DeleteOlderThanAsync(DateTimeOffset cutoff, CancellationToken ct);

    }
}
