using ProjetoAcademiaAPI.Application.Interfaces.Repositories;
using ProjetoAcademiaAPI.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        //private IApiLogRepository? _apiLogRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        /*public IApiLogRepository ApiLogRepository
        {
            get
            {
                return _apiLogRepository ??= new ApiLogRepository(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }*/

        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }
    }
}
