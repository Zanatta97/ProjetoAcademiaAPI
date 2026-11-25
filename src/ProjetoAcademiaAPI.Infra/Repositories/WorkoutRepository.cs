using Microsoft.EntityFrameworkCore;
using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Domain.Entities;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class WorkoutRepository : Repository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Workout>> ListActiveAsync(CancellationToken ct)
        {
            return await _context.Set<Workout>().AsNoTracking().Where(x => x.IsArchived == false).ToListAsync(ct);
        }
    }
}
